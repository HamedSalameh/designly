export interface AuthenticatedUser {
    name: string;
    given_name: string;
    family_name: string;
    email: string;
    tenant_id: string;
    // to be supported in the future
    roles: string[];
    permissions: string[];
    profile_image: string;
}

export interface IAuthenticationState {
AuthenticationError: string;
User?: AuthenticatedUser | null;
    IdToken: string;
    AccessToken: string;
    RefreshToken: string;
    ExpiresIn: string;
    ExpiresAt: string;
}

export const InitialAuthenticationState: IAuthenticationState = {
    AuthenticationError: '',
    User: null,
    IdToken: '',
    AccessToken: '',
    RefreshToken: '',
    ExpiresIn: '',
    ExpiresAt: ''
};