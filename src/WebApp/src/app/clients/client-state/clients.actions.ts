import { Update } from '@ngrx/entity';
import { createAction, props } from '@ngrx/store';
import { Client } from 'src/app/clients/models/client.model';

// Client selection
export const selectClient = createAction(
  '[ClientState] SelectClient',
  props<{ clientId: string }>()
);
export const unselectClient = createAction('[ClientState] UnselectClient');

// Edit/View modes
export const activateEditMode = createAction(
  '[ClientState] EditMode',
  props<{ clientId: string }>()
);
export const activateViewMode = createAction('[ClientState] ViewMode');

// API Calls
export const getClientsRequest = createAction('[ClientState] GetClients');
export const getClientsRequestSuccess = createAction(
  '[ClientState] GetClientsSuccess',
  props<{ payload: any }>()
);
export const getClientsRequestError = createAction(
  '[ClientState] GetClientsError',
  props<{ payload: any }>()
);

export const getClientRequest = createAction(
  '[ClientState] GetClient',
  props<{ clientId: string }>()
);

export const addClientRequest = createAction(
  '[ClientState] AddClient',
  props<{ draftClientModel: Client }>()
);
export const addClientRequestSuccess = createAction(
  '[ClientState] AddClientSuccess',
  props<{ payload: any }>()
);

export const deleteClientRequest = createAction(
  '[ClientState] DeleteClient',
  props<{ clientId: string }>()
);
export const deleteClientRequestSuccess = createAction(
  '[ClientState] DeleteClientSuccess',
  props<{ payload: any }>()
);

export const updateClientRequest = createAction(
  '[ClientState] UpdateClient',
  props<{ clientModel: Client }>()
);
export const updateClientRequestSuccess = createAction(
  '[ClientState] UpdateClientSuccess',
  props<{ client: Update<Client> }>()
);
