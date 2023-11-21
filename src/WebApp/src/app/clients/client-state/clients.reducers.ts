import { IClientState, InitialClientsState } from './clients.state';

import { createReducer, on } from '@ngrx/store';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import { addClientRequest, activateEditMode, activateViewMode, selectClient, unselectClient, updateSelectedClientModel } from './clients.actions';
// Reducer

export const ClientStateReducer = createReducer<IClientState>(
  InitialClientsState,
  on(addClientRequest, (state) => ({
    ...state,
    editMode: true,
    selectedClientId: NEW_CLIENT_ID,
  })),
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
  })
);