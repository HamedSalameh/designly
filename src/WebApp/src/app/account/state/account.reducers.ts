import { createReducer, on } from "@ngrx/store";
import { AccountAdapter, IAccountState, InitialAccountState } from "./account.state";
import { getAccountUsersRequestSuccess, globalResetState, resetAccountState } from "./account.actions";

export const AccountStateReducer = createReducer<IAccountState>(
    InitialAccountState,

    on(resetAccountState, (state) => ({ ...state, ...InitialAccountState })),
    on(globalResetState, (state) => ({ ...state, ...InitialAccountState })),

    on(getAccountUsersRequestSuccess, (state, {payload}) => {
        console.log('[AccountStateReducer] getAccountUsersRequestSuccess', payload);
        return AccountAdapter.setAll(payload, state);
    }),
);