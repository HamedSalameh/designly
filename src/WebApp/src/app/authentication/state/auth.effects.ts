import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { map, mergeMap, catchError, tap, switchMap, finalize } from 'rxjs/operators';
import { of } from 'rxjs';
import {
  checkAuthentication,
  checkAuthenticationFailure,
  checkAuthenticationSuccess,
  clearAuthenticationError,
  loginFailure,
  loginStart,
  loginSuccess,
  logout,
  logoutFailed,
  logoutSuccess,
  revokeTokens
} from './auth.actions';
import { AuthenticationService } from '../authentication-service.service';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { SetLoading } from 'src/app/shared/state/shared/shared.actions';

import { AuthenticatedUser } from './auth.state';
import { Strings } from 'src/app/shared/strings';
import { resetProjectsState } from 'src/app/projects/projects-state/projects.actions';
import { resetClientsState } from 'src/app/clients/client-state/clients.actions';
import { resetAccountState } from 'src/app/account/state/account.actions';

@Injectable()
export class AuthenticationEffects {
  constructor(
    private actions$: Actions,
    private authenticationService: AuthenticationService,
    private router: Router,
    private store: Store) { }

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loginStart),
      tap(() => this.store.dispatch(SetLoading(true))), // Dispatch loading before making request
      mergeMap(({ signInRequest }) =>
        this.authenticationService.signIn(signInRequest).pipe(
          map((response: AuthenticatedUser) =>
            loginSuccess({
              User: this.buildUserObject(response),
              redirect: true
            })
          ),
          catchError((error) =>
            of(loginFailure({ error })).pipe(tap(() => this.handleSigninError(error)))
          ), // Use a dedicated failure action
          finalize(() => this.store.dispatch(SetLoading(false))) // Ensure loading is reset
        )
      )
    )
  );

  loginRedirect$ = createEffect(
    () => {
      return this.actions$.pipe(
        ofType(loginSuccess),
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
        ofType(logout),
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
          mergeMap(() =>
            of(
              resetProjectsState(),
              resetClientsState(),
              resetAccountState(),
              clearAuthenticationError(), 
              revokeTokens(),
              logoutSuccess())
          ),
          catchError((error) => {
            return of(logoutFailed({ error }));
          })
        );
      })
    ));

  checkAuthentication$ = createEffect(() =>
    this.actions$.pipe(
      ofType(checkAuthentication),
      switchMap(() =>
        this.authenticationService.isAuthenticated().pipe(
          map((authenticatedUserResponse: AuthenticatedUser) => {
            return checkAuthenticationSuccess({ User: this.buildUserObject(authenticatedUserResponse) });
          }),
          catchError((error) => {
            if (error.originalError?.status === 401) {
              this.router.navigate(['/login']);
            }
            return of(checkAuthenticationFailure({ error }));
          })
        )
      )
    )
  );

  private buildUserObject(response: any): AuthenticatedUser {
    const user: AuthenticatedUser = {
      Email: response['email'],
      Name: `${response['givenName']} ${response['familyName']}`,
      GivenName: response['givenName'],
      FamilyName: response['familyName'],
      Roles: response['roles'],
      Permissions: response['permissions'],
      TenantId: response['tenantId'],
      ProfileImage: response['profileImage'],
    };
    return user;
  }

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

    this.store.dispatch(revokeTokens());
    this.store.dispatch(SetLoading(false));
    this.store.dispatch(loginFailure({ error: errorMessage }));
  }
}
