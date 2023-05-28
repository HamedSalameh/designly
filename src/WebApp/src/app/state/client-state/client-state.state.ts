// client state class
import { State } from "@ngxs/store";
import { Action, Selector, StateContext } from "@ngxs/store";
import { SelectClient, UnselectClient } from "./client-state.actions";
import { ClientStateModel } from "./client-state.models";

// the decorated class that holds the state for ClientStateModel
@State<ClientStateModel>({
    name: 'clientState',
    defaults: {
        selectedClient: null
    }
})

// Comparing to CQRS, this is similar to the CommandHandler
export class ClientState {

    @Selector()
    static selectedClient(state: ClientStateModel) {
        return state.selectedClient;
    }

    @Action(SelectClient)
    Select({getState, patchState}: StateContext<ClientStateModel>, {payload}: SelectClient) {
        const state = getState();
        patchState({
            selectedClient: payload
        });
    }

    @Action(UnselectClient)
    Unselect({getState, patchState}: StateContext<ClientStateModel>, {payload}: SelectClient) {
        const state = getState();
        patchState({
            selectedClient: payload
        });
    }
}