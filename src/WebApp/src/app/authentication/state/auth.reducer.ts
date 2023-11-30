import { Action, createReducer, on } from "@ngrx/store";
import { AuthenticationState, InitialAuthenticationState } from "./auth.state";
import { loginSuccess} from "./auth.actions";

const _authenticationReducer = createReducer(
    InitialAuthenticationState,
    
    on(loginSuccess, (state, action) => {
        return {
            ...state,
            User: action.user,
            AccessToken: action.accessToken,
            RefreshToken: action.idToken,
            ExpiresIn: action.expiresIn
        }
    
    })
);

export function AuthenticationReduce(state: AuthenticationState | undefined, action: Action) {
    return _authenticationReducer(state, action)
}