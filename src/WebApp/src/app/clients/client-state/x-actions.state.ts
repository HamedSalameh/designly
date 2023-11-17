import { createAction, props } from '@ngrx/store';
import { Client } from 'src/app/clients/models/client.model';


// Actions (Actions are basically events that are dispatched to the store)

export const SelectClient = createAction('[ClientState] SelectClient', props<{ payload: string; }>());
export const SelectedClientModel = createAction('[ClientState] SelectedClientModel');
export const UnselectClient = createAction('[ClientState] UnselectClient');
export const EditMode = createAction('[ClientState] EditMode', props<{ payload: string; }>());
export const ViewMode = createAction('[ClientState] ViewMode');

// Action to get client via api
export const getClient = createAction('[ClientState] GetClient', props<{ clientId: string; }>());
export const getClientSucess = createAction('[ClientState] GetClientSuccess', props<{ payload: any; }>());
export const getClientFailure = createAction('[ClientState] GetClientFailure', props<{ payload: any; }>());

// Action to create new client via api
export const AddClient = createAction('[ClientState] AddClient', props<{ draftClient: Client; }>());
export const AddClientSuccess = createAction('[ClientState] AddClientSuccess', props<{ payload: any; }>());
export const AddClientFailure = createAction('[ClientState] AddClientFailure', props<{ payload: any; }>());

// Action to update client via api
export const UpdateClient = createAction('[ClientState] UpdateClient', props<{ clientModel: Client; }>());
export const UpdateClientSuccess = createAction('[ClientState] UpdateClientSuccess', props<{ payload: any; }>());
export const UpdateClientFailure = createAction('[ClientState] UpdateClientFailure', props<{ payload: any; }>());

// Action to delete client via api
export const DeleteClient = createAction('[ClientState] DeleteClient', props<{ clientId: string; }>());
export const DeleteClientSuccess = createAction('[ClientState] DeleteClientSuccess', props<{ payload: any; }>());
export const DeleteClientFailure = createAction('[ClientState] DeleteClientFailure', props<{ payload: any; }>());