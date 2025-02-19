import { Action, createReducer, on } from "@ngrx/store";
import { IAuthenticationState, InitialAuthenticationState } from "./auth.state";
import { checkAuthentication, checkAuthenticationSuccess, clearAuthenticationError, loginFailure, loginSuccess, logoutSuccess, revokeTokens } from "./auth.actions";

const _authenticationReducer = createReducer<IAuthenticationState>(
    InitialAuthenticationState,

    on(loginSuccess, (state, action) => {
        return {
            ...state,
            User: action.User
        }
    }),
    on(loginFailure, (state, { error: payload }) => {
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

    on(checkAuthenticationSuccess, (state, action) => {
        return {
            ...state,
            User: action.User
        }
    }),
    on(checkAuthentication, (state) => {
        return {
            ...state
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
}