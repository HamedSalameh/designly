import { Action, createReducer, on } from "@ngrx/store";
import { InitialSharedState, SharedState } from "./shared.state";
import { SetActiveModule, SetLoading, globalResetState, resetSharedState } from "./shared.actions";

const _sharedStateReducer = createReducer(
    InitialSharedState,

    on(resetSharedState, (state) => ({ ...state, ...InitialSharedState })),
    on(globalResetState, (state) => ({ ...state, ...InitialSharedState })),

    on(SetLoading, (state, action) => {
        return {
            ...state,
            loading: action.payload
        }
    }),
    on(SetActiveModule, (state, action) => {
        return {
            ...state,
            activeModule: action.payload
        }
    }
    ));

export function SharedStateReducer(state: SharedState | undefined, action: Action) {
    return _sharedStateReducer(state, action)
}