import { Action, createAction } from "@ngrx/store";

export const SET_LOADING = '[Shared] Set Loading';

export const SetLoading = createAction(
    SET_LOADING,
    (payload: boolean) => ({ payload })
)