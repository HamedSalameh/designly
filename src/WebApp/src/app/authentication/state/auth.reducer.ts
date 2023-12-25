import { Action, createReducer, on } from "@ngrx/store";
import { AuthenticationState, InitialAuthenticationState } from "./auth.state";
import { loginSuccess, revokeTokens} from "./auth.actions";

const _authenticationReducer = createReducer(
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
    })
);

export function AuthenticationReduce(state: AuthenticationState | undefined, action: Action) {
    return _authenticationReducer(state, action)
}