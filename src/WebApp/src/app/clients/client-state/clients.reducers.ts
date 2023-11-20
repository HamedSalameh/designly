import { IClientState, InitialClientsState } from './clients.state';

import { createReducer, on } from '@ngrx/store';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import { AddClientRequest, EditModeActivated, ViewModeActivated, ClientSelectedEvent, UnselectClientEvent, UpdateSelectedClientModel } from './clients.actions';
// Reducer

export const ClientStateReducer = createReducer<IClientState>(
  InitialClientsState,
  on(AddClientRequest, (state) => ({ ...state, editMode: true, selectedClientId: NEW_CLIENT_ID })),
  on(EditModeActivated, (state, { payload }) => ({ ...state, editMode: true, draftEntity: payload })),
  on(ViewModeActivated, (state) => ({ ...state, editMode: false, draftEntity: null })),
  on(ClientSelectedEvent, (state, { payload }) => {    
    return { ...state, selectedClientId: payload };
  }),
  on(UnselectClientEvent, (state) => ({ ...state, 
    selectedClientModel: null,
    selectedClientId: null })),

  on(UpdateSelectedClientModel, (state, { payload }) => {
    return { ...state, selectedClientModel: payload };
  })
);