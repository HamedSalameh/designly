import { UserStatus } from "./user-status.enum";

export interface Member {
    id: string;
    accountId: string;  // More descriptive than just 'account'
    firstName: string;
    lastName?: string;
    email: string;
    jobTitle?: string;
    status: UserStatus;  // Consider renaming 'userStatus' to 'status' for simplicity
}
