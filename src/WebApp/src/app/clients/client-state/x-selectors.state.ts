import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IClientState } from './x-types.state';

export const CLIENTS_STATE_NAME = 'clients';

export const ClientState = createFeatureSelector<IClientState>(CLIENTS_STATE_NAME);

export const getApplicationState = createSelector(
  ClientState,
  (state: IClientState) => state
);

export const SelectedClientIdSelector = createSelector(
  ClientState,
  (state: IClientState) => {
    console.log('getSelectClient selector', state.selectedClientId);
    return state.selectedClientId;
  }
);
export const ClientSelector = createSelector(
  ClientState,
  (state: IClientState) => state.selectedClientModel
);
