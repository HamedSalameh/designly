import { createAction } from "@ngrx/store";

export const raiseApplicationError = createAction('[ErrorState] RaiseApplicationError', (payload: any) => ({ payload }));
export const raiseNetworkError = createAction('[ErrorState] RaiseNetworkError', (payload: any) => ({ payload }));
export const raiseUnknownError = createAction('[ErrorState] RaiseUnknownError', (payload: any) => ({ payload }));

export const clearApplicationError = createAction('[ErrorState] ClearApplicationError');
export const clearNetworkError = createAction('[ErrorState] ClearNetworkError');
export const clearUnknownError = createAction('[ErrorState] ClearUnknownError');

export const clearAllErrors = createAction('[ErrorState] ClearAllErrors');
