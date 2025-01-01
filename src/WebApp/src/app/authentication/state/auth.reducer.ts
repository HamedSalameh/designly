import { Action, createReducer, on } from "@ngrx/store";
import { IAuthenticationState, InitialAuthenticationState } from "./auth.state";
import { clearAuthenticationError, loginFailed, loginSuccess, logoutSuccess, revokeTokens } from "./auth.actions";
import { clearAllErrors } from "src/app/shared/state/error-state/error.actions";

const _authenticationReducer = createReducer<IAuthenticationState>(
    InitialAuthenticationState,

    on(loginSuccess, (state, action) => {
        return {
            ...state,
            User: action.User,
            IdToken: action.IdToken,
            AccessToken: action.AccessToken,
            RefreshToken: action.RefreshToken,
            ExpiresIn: action.ExpiresIn,
            ExpiresAt: action.ExpiresAt

        }
    }),
    on(loginFailed, (state, { error: payload }) => {
        return {
            ...state,
            AuthenticationError: payload
        }
    }),
    on(logoutSuccess, (state) => {
        return {
            ...state,
            User: null,
            IdToken: '',
            AccessToken: '',
            RefreshToken: '',
            ExpiresIn: '',
            ExpiresAt: ''
        }
    }),
    on(revokeTokens, (state) => {
        return {
            ...state,
            User: null,
            IdToken: '',
            AccessToken: '',
            RefreshToken: '',
            ExpiresIn: '',
            ExpiresAt: ''
        }
    }),

    on(clearAuthenticationError, (state) => {
        return {
            ...state,
            AuthenticationError: ''
        }
    })
);

export function AuthenticationReduce(state: IAuthenticationState | undefined, action: Action) {
    return _authenticationReducer(state, action)
