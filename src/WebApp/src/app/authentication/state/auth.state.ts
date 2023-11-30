export interface AuthenticationState {
    User: any;
    Token: string;
}

export const InitialAuthenticationState: AuthenticationState = {
    User: null,
    Token: ''
};