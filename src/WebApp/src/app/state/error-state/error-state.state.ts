import { Action, Actions, Selector, State, StateContext } from "@ngxs/store";
import { AddApplicationError, AddNetworkError, AddServerError, ClearApplicationError, ClearNetworkError, ClearServerError } from "./error-state.actions";
import { ErrorStateModel } from "./error-state.model";

@State<ErrorStateModel>({
    name: 'errorState',
    defaults: {
        applicationError: null,
        networkError: null,
        serverError: null
    }
  })

export class ErrorState {

    @Selector()
    static getApplicationError(state: ErrorStateModel) {
        return state.applicationError;
    }

    @Selector()
    static getNetworkError(state: ErrorStateModel) {
        console.debug('[ErrorState] [getNetworkError] ', state.networkError);
        return state.networkError;
    }

    @Selector()
    static getServerError(state: ErrorStateModel) {
        return state.serverError;
    }

    @Action(AddNetworkError)
    AddNetworkError({getState, patchState}: StateContext<ErrorStateModel>, {payload}: AddNetworkError) {
        const state = getState();
        patchState({
            networkError: payload
        });
    }

    @Action(AddApplicationError)
    AddApplicationError({getState, patchState}: StateContext<ErrorStateModel>, {payload}: AddApplicationError) {
        const state = getState();
        patchState({
            applicationError: payload
        });
    }

    @Action(AddServerError)
    AddServerError({getState, patchState}: StateContext<ErrorStateModel>, {payload}: AddServerError) {
        const state = getState();
        patchState({
            serverError: payload
        });
    }

    @Action(ClearApplicationError)
    ClearApplicationError({getState, patchState}: StateContext<ErrorStateModel>) {
        const state = getState();
        patchState({
            applicationError: null
        });
    }

    @Action(ClearNetworkError)
    ClearNetworkError({getState, patchState}: StateContext<ErrorStateModel>) {
        const state = getState();
        patchState({
            networkError: null
        });
    }

    @Action(ClearServerError)
    ClearServerError({getState, patchState}: StateContext<ErrorStateModel>) {
        const state = getState();
        patchState({
            serverError: null
        });
    }
}