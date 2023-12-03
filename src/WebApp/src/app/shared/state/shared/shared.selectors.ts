import { createFeatureSelector, createSelector } from "@ngrx/store";
import { SharedState } from "./shared.state";

export const SHARED_STATE_NAME = 'Shared_State';

export const SharedStateFeatureSelector = createFeatureSelector<SharedState>(SHARED_STATE_NAME);

export const isLoading = createSelector(
    SharedStateFeatureSelector,
    (state: SharedState) => state.loading
);