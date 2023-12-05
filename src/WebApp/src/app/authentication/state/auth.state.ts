export interface AuthenticationState {
    User: any;
    IdToken: string;
    AccessToken: string;
    RefreshToken: string;
    ExpiresIn: string;
    ExpiresAt: string;
}

export const InitialAuthenticationState: AuthenticationState = {
    User: null,
    IdToken: '',
    AccessToken: '',
    RefreshToken: '',
    ExpiresIn: '',
    ExpiresAt: ''
};