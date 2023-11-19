import { createAction, props } from '@ngrx/store';
import { Client } from 'src/app/clients/models/client.model';


// Actions (Actions are basically events that are dispatched to the store)
export const ClientSelectedEvent = createAction('[ClientState] SelectClient', props<{ payload: string; }>());
export const UnselectClientEvent = createAction('[ClientState] UnselectClient');
export const EditModeActivated = createAction('[ClientState] EditMode', props<{ payload: string; }>());
export const ViewModeActivated = createAction('[ClientState] ViewMode');

// Action to get client via api
export const GetClientRequest = createAction('[ClientState] GetClient', props<{ clientId: string; }>());
export const GetClientRequestSuccess = createAction('[ClientState] GetClientSuccess', props<{ payload: any; }>());

// Action to create new client via api
export const AddClientRequest = createAction('[ClientState] AddClient', props<{ draftClient: Client; }>());
export const AddClientRequestSuccess = createAction('[ClientState] AddClientSuccess', props<{ payload: any; }>());

// Action to update client via api
export const UpdateClientRequest = createAction('[ClientState] UpdateClient', props<{ clientModel: Client; }>());
export const UpdateClientRequestSuccess = createAction('[ClientState] UpdateClientSuccess', props<{ payload: any; }>());

// Action to delete client via api
export const DeleteClient = createAction('[ClientState] DeleteClient', props<{ clientId: string; }>());
export const DeleteClientSuccess = createAction('[ClientState] DeleteClientSuccess', props<{ payload: any; }>());