// client state class
import { Select, State } from "@ngxs/store";
import { Action, Selector, StateContext } from "@ngxs/store";
import { EditMode, SelectClient, UnselectClient, ViewMode } from "./client-state.actions";
import { ClientStateModel } from "./client-state.models";

// the decorated class that holds the state for ClientStateModel
@State<ClientStateModel>({
    name: 'clientState',
    defaults: {
        selectedClientId: null,
        editMode: false,
        draftEntity: null
    }
})

// Comparing to CQRS, this is similar to the CommandHandler
export class ClientState {

    @Selector()
    static applicationState(state: ClientStateModel) {
        return state;
    }

    @Selector()
    static selectedClient(state: ClientStateModel) {
        return state.selectedClientId;
    }

    @Action(EditMode)
    Edit({getState, patchState}: StateContext<ClientStateModel>, {payload}: EditMode) {
        const state = getState();
        patchState({
            editMode: true,
            draftEntity: payload
        });
    }

    @Action(ViewMode)
    View({getState, patchState}: StateContext<ClientStateModel>) {
        const state = getState();
        patchState({
            editMode: false,
            draftEntity: null
        });
    }

    @Action(SelectClient)
    Select({getState, patchState}: StateContext<ClientStateModel>, {payload}: SelectClient) {
        const state = getState();
        patchState({
            selectedClientId: payload
        });
    }

    @Action(UnselectClient)
    Unselect({getState, patchState}: StateContext<ClientStateModel>, {payload}: SelectClient) {
        const state = getState();
        patchState({
            selectedClientId: null
        });
    }
}