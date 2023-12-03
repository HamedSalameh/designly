import { Action, createReducer, on } from "@ngrx/store";
import { InitialSharedState, SharedState } from "./shared.state";
import { SetLoading } from "./shared.actions";

const _sharedStateReducer = createReducer(
    InitialSharedState,

    on(SetLoading, (state, action) => {
        return {
            ...state,
            loading: action.payload
        }
    })
)

export function SharedStateReducer(state: SharedState | undefined, action: Action) {
    return _sharedStateReducer(state, action)
}