import { createFeatureSelector, createSelector } from '@ngrx/store';
import { CLIENTS_STATE_NAME, ClientsAdapter, IClientState } from './clients.state';

export const ClientState = createFeatureSelector<IClientState>(CLIENTS_STATE_NAME);

export const getApplicationState = createSelector(
  ClientState,
  (state: IClientState) => state
);

export const getSelectedClientIdFromState = createSelector(
  ClientState,
  (state: IClientState) => state.selectedClientId
);

export const getViewModeFromState = createSelector(
  ClientState,
  (state: IClientState) => state.editMode
);

export const clientsSelector = ClientsAdapter.getSelectors();

export const getClients = createSelector(ClientState, clientsSelector.selectAll);
export const getClientsEntities = createSelector(ClientState, clientsSelector.selectEntities);
export const getSingleClient = createSelector(
  getClientsEntities,
  getSelectedClientIdFromState,
  (clients, clientId) => {
    return clientId ? clients[clientId] : null;
  }
)