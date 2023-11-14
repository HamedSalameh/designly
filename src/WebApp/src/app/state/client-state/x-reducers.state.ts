import { xInitialClientsState } from './x-types.state';

import { createReducer, on } from '@ngrx/store';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import { IClientState } from 'src/app/shared/models/client-state.interface';
import { xAddClient, xEditMode, xViewMode, xSelectClient, xUnselectClient, getClientSucess, getClientFailure, xSelectedClientModel } from './x-actions.state';


// Reducer

export const xClientStateReducer = createReducer<IClientState>(
  xInitialClientsState,
  on(xAddClient, (state) => ({ ...state, editMode: true, selectedClientId: NEW_CLIENT_ID })),
  on(xEditMode, (state, { payload }) => ({ ...state, editMode: true, draftEntity: payload })),
  on(xViewMode, (state) => ({ ...state, editMode: false, draftEntity: null })),
  on(xSelectClient, (state, { payload }) => {
    console.log('xSelectClient action', payload);
    return { ...state, selectedClientId: payload };
  }),
  on(xUnselectClient, (state) => ({ ...state, selectedClientId: null })),

  on(getClientSucess, (state, { payload }) => (
    { ...state, selectedClientModel: payload })),
  on(getClientFailure, (state, { payload }) => (
    { ...state, selectedClientModel: payload }))
);
