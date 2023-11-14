import { createAction, props } from '@ngrx/store';


// Actions

export const xSelectClient = createAction('[ClientState] SelectClient', props<{ payload: string; }>());
export const xSelectedClientModel = createAction('[ClientState] SelectedClientModel');
export const xUnselectClient = createAction('[ClientState] UnselectClient');
export const xEditMode = createAction('[ClientState] EditMode', props<{ payload: string; }>());
export const xViewMode = createAction('[ClientState] ViewMode');
export const xAddClient = createAction('[ClientState] AddClient');

export const getClient = createAction('[ClientState] GetClient', props<{ clientId: string; }>());
export const getClientSucess = createAction('[ClientState] GetClientSuccess', props<{ payload: any; }>());
export const getClientFailure = createAction('[ClientState] GetClientFailure', props<{ payload: any; }>());
