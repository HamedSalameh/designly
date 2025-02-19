export interface AuthenticatedUser {
    Name: string;
    GivenName: string;
    FamilyName: string;
    Email: string;
    TenantId: string;
    // to be supported in the future
    Roles: string[];
    Permissions: string[];
    ProfileImage: string;
}


export interface IAuthenticationState {
AuthenticationError: string;
User?: AuthenticatedUser | null;
}

export const InitialAuthenticationState: IAuthenticationState = {
    AuthenticationError: '',
    User: null
};