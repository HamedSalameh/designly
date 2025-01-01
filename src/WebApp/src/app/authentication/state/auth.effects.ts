import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { map, mergeMap, catchError, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import {
  clearAuthenticationError,
  loginFailed,
  loginStart,
  loginSuccess,
  logout,
  logoutFailed,
  logoutSuccess,
  revokeTokens
} from './auth.actions';
import { AuthenticationService } from '../authentication-service.service';
import { Router } from '@angular/router';
import { SigninResponse } from '../models/signin-response.model';
import * as moment from 'moment';
import { Store } from '@ngrx/store';
import { SetLoading } from 'src/app/shared/state/shared/shared.actions';
import { Buffer } from 'buffer';
import { AuthenticatedUser } from './auth.state';
import { HttpErrorHandlingService } from 'src/app/shared/services/error-handling.service';
import { Strings } from 'src/app/shared/strings';

@Injectable()
export class AuthenitcationEffects {
  constructor(
    private actions$: Actions,
    private authenticationService: AuthenticationService,
    private router: Router,
    private store: Store) { }

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loginStart),
      mergeMap((action) => {
        const signInRequest = action.signInRequest;
        return this.authenticationService.signIn(signInRequest).pipe(
          map((response: SigninResponse) => {
            const accessToken = response.accessToken;
            const idToken = response.idToken;
            const expiresIn = response.expiresIn;
            const expiresAt = moment().add(response.idToken, 'second');
            this.store.dispatch(SetLoading(false));
            return loginSuccess({
              User: this.decodeIdToken(idToken),
              IdToken: idToken,
              AccessToken: accessToken,
              RefreshToken: '',
              ExpiresIn: expiresIn,
              ExpiresAt: expiresAt,
              redirect: true
            });
          }),
          catchError((error) => {
            this.store.dispatch(SetLoading(false));
            this.handleSigninError(error);
            return of();
          })
        );
      })
    )
  );

  private handleSigninError(error: any) {
    const serverResponse = error.originalError?.status || 500; // Default to 500
    const serverErrorMessage = error.originalError?.error || 'Unexpected Error';

    const errorMessages: Record<number, string> = {
      400: Strings.BadRequest,
      401: Strings.Unauthorized,
      403: Strings.Forbidden,
      404: Strings.NotFound,
      500: Strings.InternalServerError,
      503: Strings.ServiceUnavailable,
    };

    let errorMessage = errorMessages[serverResponse] || Strings.UnknownError;

    switch (serverResponse) {
      case 401:
        if (serverErrorMessage === 'Incorrect username or password.') {
          errorMessage = Strings.InvalidCredentials;
        } else if (serverErrorMessage === 'Password attempts exceeded') {
          errorMessage = Strings.PasswordAttemptsExceeded;
        }
        break;
      case 500:
        errorMessage = serverErrorMessage;
        break;
      case 403:
        error.message = Strings.Forbidden;
        break;
      case 404:
        error.message = Strings.NotFound;
        break;
      case 500:
        error.message = Strings.InternalServerError;
        break;
      case 503:
        error.message = Strings.ServiceUnavailable;
        break;
      default:
        error.message = Strings.UnexpectedError;
        break;
    }

    const application = {
      message: errorMessage,
      originalError: error.originalError,
      handled: false,
      type: 'ApplicationError',
    };

    this.store.dispatch(revokeTokens());
    this.store.dispatch(SetLoading(false));
    this.store.dispatch(loginFailed({ error: errorMessage }));
  }

  loginRedirect$ = createEffect(
    () => {
      return this.actions$.pipe(
        ofType(...[loginSuccess]),
        tap((action) => {
          if (action.redirect) {
            this.router.navigate(['/']);
          }

        })
      );
    },
    { dispatch: false }
  );

  logoutRedirect$ = createEffect(
    () => {
      return this.actions$.pipe(
        ofType(...[logout]),
        tap(() => {
          this.router.navigate(['/login']);
        })
      );
    },
    { dispatch: false }
  );

  logout$ = createEffect(() =>
    this.actions$.pipe(
      ofType(logout),
      mergeMap(() => {
        return this.authenticationService.signOut().pipe(
          map(() => {
            this.store.dispatch(clearAuthenticationError());
            this.store.dispatch(revokeTokens());
            return logoutSuccess();
          }),
          catchError((error) => {
            return of(logoutFailed({ error }));
          })
        );
      })
    ));

  private decodeIdToken(idToken: string): AuthenticatedUser {
    const decodedToken = Buffer.from(idToken.split('.')[1], 'base64').toString();
    const parsedToken = JSON.parse(decodedToken);

    const tenantClaim = parsedToken['cognito:groups']?.find((group: string) => group.startsWith('tenant_'));
    const tenant_id: string | undefined = tenantClaim?.substring('tenant_'.length);

    const user: AuthenticatedUser = {
      email: parsedToken.email,
      family_name: parsedToken.family_name,
      given_name: parsedToken.given_name,
      name: parsedToken.given_name + ' ' + parsedToken.family_name,
      tenant_id: tenant_id || "",
      profile_image: parsedToken.profile_image || "",
      roles: parsedToken.roles || [],
      permissions: parsedToken.permissions || []
    }

    return user;
  }
}
