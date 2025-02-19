import { createFeatureSelector, createSelector } from "@ngrx/store";
import { IAuthenticationState } from "./auth.state";
import { Buffer } from 'buffer';

export const AUTH_STATE_NAME = 'auth';

export const authenticationFeatureSelector = createFeatureSelector<IAuthenticationState>(AUTH_STATE_NAME);

export const getUser = createSelector(
    authenticationFeatureSelector,
    (state: IAuthenticationState) => state.User
)

export const isAuthenticated = createSelector(
    authenticationFeatureSelector,
    (state: IAuthenticationState) => !!state.User
)

export const getTenantId = createSelector(
    authenticationFeatureSelector,
    (state: IAuthenticationState) => state.User?.TenantId
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