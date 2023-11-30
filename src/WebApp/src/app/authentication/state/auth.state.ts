export interface AuthenticationState {
    User: any;
    Token: string;

    AccessToken: string;
    RefreshToken: string;
    ExpiresIn: string;
}

export const InitialAuthenticationState: AuthenticationState = {
    User: null,
    Token: '',

    AccessToken: '',
    RefreshToken: '',
    ExpiresIn: ''
};