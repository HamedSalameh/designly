# Accounts and Account Management

## Use Cases

### User Registration

1. redirect to the registration page.
2. The user selects a plan.
3. The user enters their email address and password.
4. A new user is created in the AWS Cognito user pool.
5. The user is sent a confirmation email.
6. The user confirms their email address by clicking on the link in the email.
7. The link in the email redircs the user to the application login page.

### Invite User to Account

1. The user clicks on the `Invite User` button.
2. The user enters the email address of the user to invite.
3. The user clicks on the `Send Invitation` button.
4. The user is sent an invitation email.
5. AWS: A new user is created in the AWS Cognito user pool
6. AWS: The user is added to the tenant group.

7. The invited user clicks on the link in the email.
8. The link in the email redirects the user to the application join by invite page.
9. The user enters their email address, sets their password and general details.
10. The user is authenticated against the AWS Cognito user pool.
11. The user is authenticated against the application database.
12. redirect to the dashboard.

### User Registration with Invitation

1. The user receives an invitation email.
2. The user clicks on the link in the email.
3. The link in the email redirects the user to the application login page.
4. The user enters their email address and password.
5. The user is authenticated against the AWS Cognito user pool.
6. The user is authenticated against the application database.
7. Redirect to dashboard.

### User Signin

1. The user enters their email address and password.
2. The user is authenticated against the AWS Cognito user pool.
3. The user is authenticated against the application database.
4. If this is the first time the user has signed in, then:  
   0. If the user does not have a tenant, then this mean new account creation
   1. Basic account is created
   2. The user is assigned as the owner of the account.
   3. A default team is created for the account.
   4. the user is added to the default team.
   5. the account status is set to `Active`.
   6. AWS: A new group is created for the account (to hold the tenant)
   7. AWS: The user is added to the account group.
   8. AWS: The user is added to the `AccountOwner` group.
   9. A welcome email is sent to the user.
5. redirect to the dashboard.



## User Status

The `UserStatus` enum defines the possible states a user can be in within the application.

- **BeforeActivation**: Registered users that are not activated yet or have not confirmed their email address.
  
- **Active**: Users that are currently in use.

- **Suspended**: Users that are not in use but can be reactivated. Suspension can occur for various reasons, including non-payment, user or admin requests, and manual admin intervention.

- **Disabled**: Users that are not in use and cannot be reactivated. Similar to suspension, disabling can occur for reasons such as non-payment, user or admin requests, and manual admin intervention.

- **MarkedForDeletion**: Users marked for deletion by the application. They are not deleted immediately but after a certain period.

- **Deleted**: Users that have been deleted.

## Team Status

The `TeamStatus` enum defines the possible states a team can be in within the application.

- **Active**: The team is currently in use.

- **Suspended**: The team is not in use but can be reactivated.

- **Disabled**: The team is not in use and cannot be reactivated.

- **MarkedForDeletion**: The team is marked for deletion by the application.

- **Deleted**: The team has been deleted.

## Account Status

The `AccountStatus` enum defines the possible states an account can be in within the application.

- **Active**: The account is currently in use.

- **Suspended**: The account is not in use but can be reactivated. Suspension can occur for reasons such as non-payment, user or admin requests, and manual admin intervention.

- **Disabled**: The account is not in use and cannot be reactivated. Similar to suspension, disabling can occur for reasons such as non-payment, user or admin requests, and manual admin intervention.

- **MarkedForDeletion**: The account is marked for deletion by the application. It is not deleted immediately but after a certain period.

- **Deleted**: The account has been deleted.

## Conclusion

Understanding the different status enums is crucial for managing the state of entities in the application. Developers should use these enums consistently to represent the status of users, teams, and accounts throughout the system.
