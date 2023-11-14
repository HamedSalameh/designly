import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/models/application-state.interface.';
import { IClientState } from 'src/app/shared/models/client-state.interface';

export const ClientState = createFeatureSelector<IClientState>('clients');
export const selectFeature = (state: IApplicationState) => state.clients;

export const getApplicationState = createSelector(
  ClientState,
  (state: IClientState) => state
);

export const SelectedClientIdSelector = createSelector(
  selectFeature,
  (state: IClientState) => {
    console.log('getSelectClient selector', state.selectedClientId);
    return state.selectedClientId;
  }
);
export const ClientSelector = createSelector(
  selectFeature,
  (state: IClientState) => state.selectedClientModel
);
