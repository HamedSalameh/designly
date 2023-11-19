import { IClientState, xInitialClientsState } from './x-types.state';

import { createReducer, on } from '@ngrx/store';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import { AddClientRequest, EditModeActivated, ViewModeActivated, ClientSelectedEvent, UnselectClientEvent, GetClientRequestSuccess, UpdateClientRequestSuccess } from './x-actions.state';
// Reducer

export const xClientStateReducer = createReducer<IClientState>(
  xInitialClientsState,
  on(AddClientRequest, (state) => ({ ...state, editMode: true, selectedClientId: NEW_CLIENT_ID })),
  on(EditModeActivated, (state, { payload }) => ({ ...state, editMode: true, draftEntity: payload })),
  on(ViewModeActivated, (state) => ({ ...state, editMode: false, draftEntity: null })),
  on(ClientSelectedEvent, (state, { payload }) => {
    console.log('xSelectClient action', payload);
    return { ...state, selectedClientId: payload };
  }),
  on(UnselectClientEvent, (state) => ({ ...state, 
    selectedClientModel: null,
    selectedClientId: null })),

    
  on(GetClientRequestSuccess, (state, { payload }) => {
    console.log('getClientSucess action', payload);

    return { ...state, selectedClientModel: payload };
  }),

  on(UpdateClientRequestSuccess, (state, { payload }) => {
    console.log('updateClientSucess action', payload);
    return { ...state, selectedClientModel: payload };
  })
);