import { Client } from "src/app/clients/models/client.model";

export interface IClientState {
    selectedClientId: string | null;
    editMode: boolean;
    draftEntity: any;
    selectedClientModel: Client | null | undefined;
  }