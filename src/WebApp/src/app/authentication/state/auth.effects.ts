import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { map, mergeMap, catchError, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import {
  loginFailed,
  loginStart,
  loginSuccess,
  logout
} from './auth.actions';
import { AuthenticationService } from '../authentication-service.service';
import { Router } from '@angular/router';
import { SigninResponse } from '../models/signin-response.model';
import * as moment from 'moment';

@Injectable()
export class AuthenitcationEffects {
  constructor(
    private actions$: Actions,
    private authenticationService: AuthenticationService,
    private router: Router
  ) {}

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
            return loginSuccess({ user: signInRequest.username, accessToken, idToken, expiresIn, expiresAt, redirect: true });          
          }),
          catchError((error) => {
            return of(loginFailed({ error }));
          })
        );
      })
    )
  );

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

  logout$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(logout),
        tap(() => {
          //this.cookieService.remove("token");
          this.router.navigateByUrl('/login');
        })
      ),
    { dispatch: false }
  );

}
