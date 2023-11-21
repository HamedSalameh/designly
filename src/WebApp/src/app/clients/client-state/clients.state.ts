import { Client } from "src/app/clients/models/client.model";

export interface IClientState {
  selectedClientId: string | null;
  editMode: boolean;
  //draftEntity: any;
  selectedClientModel: Client | null | undefined;
}

export const InitialClientsState: IClientState = {
  selectedClientId: null,
  editMode: false,
  //draftEntity: null,
  selectedClientModel: null,
};