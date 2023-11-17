import { createReducer, on } from "@ngrx/store";
import { clearAllErrors, clearApplicationError, clearNetworkError, clearUnknownError, raiseApplicationError, raiseNetworkError, raiseUnknownError } from "./error.actions";
import { initialErrorStateModel } from "./error.state";

export const ErrorStateReducer = createReducer(
    initialErrorStateModel,
    on(raiseApplicationError, (state, { payload }) => ({ ...state, applicationError: payload })),
    on(raiseNetworkError, (state, { payload }) => ({ ...state, networkError: payload })),
    on(raiseUnknownError, (state, { payload }) => ({ ...state, unknownError: payload })),

    on(clearApplicationError, (state) => ({ ...state, applicationError: null })),
    on(clearNetworkError, (state) => ({ ...state, networkError: null })),
    on(clearUnknownError, (state) => ({ ...state, unknownError: null })),

    on(clearAllErrors, (state) => ({ ...state, applicationError: null, networkError: null, unknownError: null }))
)