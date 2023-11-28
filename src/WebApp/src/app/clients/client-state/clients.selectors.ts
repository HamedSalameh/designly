import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ClientsAdapter, IClientState } from './clients.state';

export const CLIENTS_STATE_NAME = 'clients';

export const ClientState = createFeatureSelector<IClientState>(CLIENTS_STATE_NAME);

export const getApplicationState = createSelector(
  ClientState,
  (state: IClientState) => state
);

export const getSelectedClientIdFromState = createSelector(
  ClientState,
  (state: IClientState) => {
    console.log('getSelectClient selector', state.selectedClientId);
    return state.selectedClientId;
  }
);

// export const getSelectedClientFromState = createSelector(
//   ClientState,
//   (state: IClientState) => state.selectedClientModel
// );

export const getViewModeFromState = createSelector(
  ClientState,
  (state: IClientState) => state.editMode
);

///////////////////////////////////////////////////////////
// refactor using NGRX Entity
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