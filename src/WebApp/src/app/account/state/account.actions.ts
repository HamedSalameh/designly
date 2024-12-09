import { createAction, props } from "@ngrx/store";
import { SearchAccountUsersRequest } from "../models/search-account-users.request";

export const resetAccountState = createAction('[Account] Reset Account State');

export const globalResetState = createAction('[Account] Global Reset State');

export const getAccoundIdRequest = createAction('[Account] Get Account ID Request');
export const getAccountIdRequestSuccess = createAction('[Account] Get Account ID Request Success',
    (payload: string) => ({ payload }));

export const getAccountUsersRequest = createAction(
    '[AccountState] Get Account Users Request',
    props<{ searchUsersRequest: SearchAccountUsersRequest }>());

export const getAccountUsersRequestSuccess = createAction(
    '[AccountState] Get Account Users Request Success',
    props<{ payload: any }>()
);

export const getAccountUsersRequestError = createAction(
    '[AccountState] Get Account Users Request Error',
    props<{ payload: any }>()
);