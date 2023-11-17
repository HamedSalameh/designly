import { IClientState, xInitialClientsState } from './x-types.state';

import { createReducer, on } from '@ngrx/store';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import { AddClient, EditMode, ViewMode, SelectClient, UnselectClient, getClientSucess, getClientFailure, SelectedClientModel } from './x-actions.state';

// Reducer

export const xClientStateReducer = createReducer<IClientState>(
  xInitialClientsState,
  on(AddClient, (state) => ({ ...state, editMode: true, selectedClientId: NEW_CLIENT_ID })),
  on(EditMode, (state, { payload }) => ({ ...state, editMode: true, draftEntity: payload })),
  on(ViewMode, (state) => ({ ...state, editMode: false, draftEntity: null })),
  on(SelectClient, (state, { payload }) => {
    console.log('xSelectClient action', payload);
    return { ...state, selectedClientId: payload };
  }),
  on(UnselectClient, (state) => ({ ...state, 
    selectedClientModel: null,
    selectedClientId: null })),

    
  on(getClientSucess, (state, { payload }) => {
    console.log('getClientSucess action', payload);
    return { ...state, selectedClientModel: payload };
  }),
  on(getClientFailure, (state, { payload }) => 
  {
    console.log('getClientFailure action', payload);
    console.log('Should update error state here');
    return { ...state, selectedClientModel: null }
  }))
