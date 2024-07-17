import { IClientState } from "src/app/clients/client-state/clients.state";
import { IErrorState } from "./error-state/error.state";

export interface IApplicationState {
    CLIENTS_STATE_NAME: IClientState;
    PROJECTS_STATE_NAME: IClientState;
    ERROR_STATE_NAME: IErrorState;
}