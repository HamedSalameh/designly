export enum UserStatus {
    BeforeActivation = 'BeforeActivation',// Registered but not activated
    Active = 'Active',// Active users
    Suspended = 'Suspended',// Suspended users
    Disabled = 'Disabled',// Disabled users
    MarkedForDeletion = 'MarkedForDeletion',// Marked for deletion
    Deleted = 'Deleted',// Deleted users
    Blacklisted = 'Blacklisted' // Blacklisted users
}
