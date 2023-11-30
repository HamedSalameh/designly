import { createFeatureSelector, createSelector } from "@ngrx/store";
import { AuthenticationState } from "./auth.state";

export const AUTH_STATE_NAME = 'auth';

export const authenticationFeatureSelector = createFeatureSelector<AuthenticationState>(AUTH_STATE_NAME);

export const getToken = createSelector(
    authenticationFeatureSelector,
    (state: AuthenticationState) => state.Token
);

export const getUser = createSelector(
    authenticationFeatureSelector,
    (state: AuthenticationState) => state.User
)

export const isAuthenticated = createSelector(
    authenticationFeatureSelector,
    (state: AuthenticationState) => !!state.Token
)