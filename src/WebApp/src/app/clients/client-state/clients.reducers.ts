import { ClientsAdapter, IClientState, InitialClientsState } from './clients.state';

import { createReducer, on } from '@ngrx/store';
import { activateEditMode, activateViewMode, selectClient, unselectClient, getClientsRequestSuccess, getClientsRequestError, addClientRequestSuccess, updateClientRequestSuccess, deleteClientRequestSuccess, resetClientsState } from './clients.actions';
import { globalResetState } from 'src/app/shared/state/shared/shared.actions';
// Reducer

export const ClientStateReducer = createReducer<IClientState>(
  InitialClientsState,

  on(resetClientsState, (state) => ({ ...state, ...InitialClientsState })),
  on(globalResetState, (state) => ({ ...state, ...InitialClientsState })),

  on(activateEditMode, (state, { clientId: payload }) => ({
    ...state,
    editMode: true,
    selectedClientId: payload,
  })),
  on(activateViewMode, (state) => ({ ...state, editMode: false })),
  on(selectClient, (state, { clientId: payload }) => {
    return { ...state, selectedClientId: payload };
  }),
  on(unselectClient, (state) => ({
    ...state,
    selectedClientModel: null,
    selectedClientId: null,
  })),

  // GET CLIENTS
  on(getClientsRequestSuccess, (state, { payload }) => {
    return ClientsAdapter.setAll(payload, state);
  }),
  on(getClientsRequestError, (state, { payload }) => {
    return { ...state, error: payload };
  }),

  // DELETE CLIENT
  on(deleteClientRequestSuccess, (state, { payload }) => {
    return ClientsAdapter.removeOne(payload.id, state);
  }),

  // ADD CLIENT
  on(addClientRequestSuccess, (state, { payload }) => {
    return ClientsAdapter.addOne(payload, state);
  }),

  // UPDATE CLIENT
  on(updateClientRequestSuccess, (state, payload) => {
    return ClientsAdapter.updateOne(
      payload.client, state);
  })
);

