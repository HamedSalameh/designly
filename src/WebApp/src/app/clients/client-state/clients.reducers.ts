import { ClientsAdapter, IClientState, InitialClientsState } from './clients.state';

import { createReducer, on } from '@ngrx/store';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import { addClientRequest, activateEditMode, activateViewMode, selectClient, unselectClient, updateSelectedClientModel, getClientsRequestSuccess, getClientsRequestError, addClientRequestSuccess, updateClientRequestSuccess, deleteClientRequestSuccess } from './clients.actions';
// Reducer

export const ClientStateReducer = createReducer<IClientState>(
  InitialClientsState,

  on(activateEditMode, (state, { payload }) => ({
    ...state,
    editMode: true,
    selectedClientId: payload,
  })),
  on(activateViewMode, (state) => ({ ...state, editMode: false })),
  on(selectClient, (state, { payload }) => {
    return { ...state, selectedClientId: payload };
  }),
  on(unselectClient, (state) => ({
    ...state,
    selectedClientModel: null,
    selectedClientId: null,
  })),

  on(updateSelectedClientModel, (state, { payload }) => {
    return { ...state, selectedClientModel: payload };
  }),

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
);