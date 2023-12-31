import { Action, createAction } from "@ngrx/store";

export const SET_LOADING = '[Shared] Set Loading';
export const SET_ACTIVE_MODULE = '[Shared] Set Active Module';

export const SetLoading = createAction(
    SET_LOADING,
    (payload: boolean) => ({ payload })
)

export const SetActiveModule = createAction(
    SET_ACTIVE_MODULE,
    (payload: string) => ({ payload })
)