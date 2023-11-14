import { IClientState } from 'src/app/shared/models/client-state.interface';


export const xInitialClientsState: IClientState = {
  selectedClientId: null,
  editMode: false,
  draftEntity: null,
  selectedClientModel: null,
};
// Selectors

export interface ClientFeatureState {
  clientState: IClientState;
}
