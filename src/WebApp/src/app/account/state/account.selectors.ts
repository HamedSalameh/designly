import { createFeatureSelector, createSelector } from "@ngrx/store";
import { ACCOUNT_STATE_NAME, IAccountState } from "./account.state";

// Define a feature selector for the account state
export const AccountData = createFeatureSelector<IAccountState>(ACCOUNT_STATE_NAME);

// Define a selector to get the account ID from the account state
export const getAccountIdFromState = createSelector(
    AccountData,
    (state: IAccountState) => state.accountId
    );

// Define a selector to get the account users from the account state
export const getAccountUsersFromState = createSelector(
    AccountData,
    (state: IAccountState) => state.accountUsers
    );

