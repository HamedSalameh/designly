import { createReducer, on } from "@ngrx/store";
import { IAccountState, InitialAccountState } from "./account.state";
import { getAccountIdRequestSuccess, getAccountUsersRequestSuccess, globalResetState, resetAccountState } from "./account.actions";

export const AccountStateReducer = createReducer<IAccountState>(
    InitialAccountState,

    on(resetAccountState, (state) => ({ ...state, ...InitialAccountState })),
    on(globalResetState, (state) => ({ ...state, ...InitialAccountState }))
);