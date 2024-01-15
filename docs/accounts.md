# Status Enums Documentation

## Introduction

This document provides an overview of the status enums used in the application to represent the status of users, teams, and accounts. Status enums are commonly used to indicate the state or condition of entities within a system.

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
