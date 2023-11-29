import { ClientsAdapter, IClientState, InitialClientsState } from './clients.state';

import { createReducer, on } from '@ngrx/store';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import { addClientRequest, activateEditMode, activateViewMode, selectClient, unselectClient, getClientsRequestSuccess, getClientsRequestError, addClientRequestSuccess, updateClientRequestSuccess, deleteClientRequestSuccess } from './clients.actions';
// Reducer

export const ClientStateReducer = createReducer<IClientState>(
  InitialClientsState,

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
  on(updateClientRequestSuccess, (state, payload ) => {
    return ClientsAdapter.updateOne(
      payload.client, state);
    })
);

