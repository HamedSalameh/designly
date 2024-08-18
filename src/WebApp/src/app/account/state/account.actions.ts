import { createAction } from "@ngrx/store";

export const resetAccountState = createAction('[Account] Reset Account State');

export const globalResetState = createAction('[Account] Global Reset State');

export const getAccoundIdRequest = createAction('[Account] Get Account ID Request');
export const getAccountIdRequestSuccess = createAction('[Account] Get Account ID Request Success', 
    (payload: string) => ({ payload }));

export const getAccountUsersRequest = createAction('[Account] Get Account Users Request');
export const getAccountUsersRequestSuccess = createAction('[Account] Get Account Users Request Success',
    (payload: any) => ({ payload }));