import { createFeatureSelector, createSelector } from "@ngrx/store";
import { IErrorState } from "./error.state";

export const ERROR_STATE_NAME = 'error';

export const ErrorState = createFeatureSelector<IErrorState>(ERROR_STATE_NAME);

export const getNetworkError = createSelector(
    ErrorState,
    (state: IErrorState) => state.networkError
);

export const getApplicationError = createSelector(
    ErrorState,
    (state: IErrorState) => state.applicationError
);

export const getUnknownError = createSelector(
    ErrorState,
    (state: IErrorState) => state.unknownError
);