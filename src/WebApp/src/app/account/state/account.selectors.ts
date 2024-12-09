import { createFeatureSelector, createSelector } from "@ngrx/store";
import { ACCOUNT_STATE_NAME, AccountAdapter, IAccountState } from "./account.state";

// Define a feature selector for the account state
export const AccountState = createFeatureSelector<IAccountState>(ACCOUNT_STATE_NAME);

// Define the entity selectors using the adapter
const { selectAll, selectEntities } = AccountAdapter.getSelectors();

// Define a selector to get the account users from the account state
export const getAccountUsersFromState = createSelector(
    AccountState,
    selectAll);

// Define a selector to get the account user by id from the account state
export const getAccountUserById = (id: string) => createSelector(
    AccountState,
    (state) => selectEntities(state)[id]
);
