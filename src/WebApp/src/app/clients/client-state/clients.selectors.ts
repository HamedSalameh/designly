import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IClientState } from './clients.state';

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

export const getSelectedClientFromState = createSelector(
  ClientState,
  (state: IClientState) => state.selectedClientModel
);

export const getViewModeFromState = createSelector(
  ClientState,
  (state: IClientState) => state.editMode
);