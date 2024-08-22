import { createFeatureSelector, createSelector } from "@ngrx/store";
import { ACCOUNT_STATE_NAME, AccountAdapter, IAccountState } from "./account.state";

// Define a feature selector for the account state
export const AccountState = createFeatureSelector<IAccountState>(ACCOUNT_STATE_NAME);

// Define account users selector
export const accountUsersSelector = AccountAdapter.getSelectors();

// Define a selector to get the account users from the account state
export const getAccountUsersFromState = createSelector(
    AccountState,
    accountUsersSelector.selectAll);
