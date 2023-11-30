import { Action, createReducer, on } from "@ngrx/store";
import { AuthenticationState, InitialAuthenticationState } from "./auth.state";
import { loginSuccess, revokeToken, setToken, setUser } from "./auth.actions";

const _authenticationReducer = createReducer(
    InitialAuthenticationState,
    on(setUser, (state, action) => ({ ...state, User: action.user })),
    on(setToken, (state, action) => ({ ...state, Token: action.token })),
    on(revokeToken, (state) => ({ ...state, Token: '' })),
    
    on(loginSuccess, (state, action) => ({ ...state, User: action.user, Token: action.token }))
);

export function AuthenticationReduce(state: AuthenticationState | undefined, action: Action) {
    return _authenticationReducer(state, action)
}