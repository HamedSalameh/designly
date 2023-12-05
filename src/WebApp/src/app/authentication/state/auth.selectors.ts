import { createFeatureSelector, createSelector } from "@ngrx/store";
import { AuthenticationState } from "./auth.state";
import { Buffer } from 'buffer';

export const AUTH_STATE_NAME = 'auth';

export const authenticationFeatureSelector = createFeatureSelector<AuthenticationState>(AUTH_STATE_NAME);

export const getIdToken = createSelector(
    authenticationFeatureSelector,
    (state: AuthenticationState) => state.IdToken
);

export const getUser = createSelector(
    authenticationFeatureSelector,
    (state: AuthenticationState) => state.User
)

export const isAuthenticated = createSelector(
    authenticationFeatureSelector,
    (state: AuthenticationState) => !!state.AccessToken && !isTokenExpired(state.AccessToken)
)

function isTokenExpired(token: string) {
    const decodedToken = Buffer.from(token.split('.')[1], 'base64').toString();
    const expiry = JSON.parse(decodedToken).exp;
    return (Math.floor((new Date).getTime() / 1000)) >= expiry;
  }