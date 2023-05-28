import { Client } from "src/app/clients/models/client.model";

// the state model
export class ClientStateModel {
  selectedClient: Client | null = null;
}
