import { createAction } from '@ngrx/store';
import { INetworkError } from '../../types';

export const raiseApplicationError = createAction(
  '[ErrorState] RaiseApplicationError',
  (payload: any) => ({ payload })
);

export const raiseNetworkError = createAction(
  '[ErrorState] RaiseNetworkError',
  (payload: INetworkError) => ({
    message: payload.message,
    handled: payload.handled,
    originalError: payload.originalError,
  })
);

export const raiseUnknownError = createAction(
  '[ErrorState] RaiseUnknownError',
  (payload: INetworkError) => ({
    message: payload.message,
    handled: payload.handled,
    originalError: payload.originalError,
  })
);

export const clearApplicationError = createAction(
  '[ErrorState] ClearApplicationError'
);
export const clearNetworkError = createAction('[ErrorState] ClearNetworkError');
export const clearUnknownError = createAction('[ErrorState] ClearUnknownError');

export const clearAllErrors = createAction('[ErrorState] ClearAllErrors');
