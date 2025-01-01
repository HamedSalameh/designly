import { createFeatureSelector, createSelector } from "@ngrx/store";
import { IAuthenticationState } from "./auth.state";
import { Buffer } from 'buffer';

export const AUTH_STATE_NAME = 'auth';

export const authenticationFeatureSelector = createFeatureSelector<IAuthenticationState>(AUTH_STATE_NAME);

export const getIdToken = createSelector(
    authenticationFeatureSelector,
    (state: IAuthenticationState) => state.IdToken
);

export const getUser = createSelector(
    authenticationFeatureSelector,
    (state: IAuthenticationState) => state.User
)

export const isAuthenticated = createSelector(
    authenticationFeatureSelector,
    (state: IAuthenticationState) => !!state.AccessToken && !isTokenExpired(state.AccessToken)
)
export const getAccessToken = createSelector(
    authenticationFeatureSelector,
    (state: IAuthenticationState) => state.AccessToken
)

export const getTenantId = createSelector(
    authenticationFeatureSelector,
    (state: IAuthenticationState) => state.User?.tenant_id
)

export const getLoginFailedError = createSelector(
    authenticationFeatureSelector,
    (state: IAuthenticationState) => state.AuthenticationError
)

function isTokenExpired(token: string) {
    const decodedToken = Buffer.from(token.split('.')[1], 'base64').toString();
    const expiry = JSON.parse(decodedToken).exp;
    return (Math.floor((new Date).getTime() / 1000)) >= expiry;
}