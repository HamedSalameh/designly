import { EntityState, createEntityAdapter } from "@ngrx/entity";
import { Client } from "src/app/clients/models/client.model";

export interface IClientState extends EntityState<Client> {
  selectedClientId: string | null;
  editMode: boolean;
  selectedClientModel: Client | null | undefined;
}

export const ClientsAdapter = createEntityAdapter<Client>({
  selectId: (client: Client) => client.Id,
  sortComparer: false,
});

export const InitialClientsState: IClientState = ClientsAdapter.getInitialState({
  selectedClientId: null,
  editMode: false,
  selectedClientModel: null,
});

// export const InitialClientsState: IClientState = {
//   selectedClientId: null,
//   editMode: false,
//   //draftEntity: null,
//   selectedClientModel: null,
// };


// refactor using NGRX Entity