import { IClientState } from "src/app/clients/client-state/x-types.state";
import { IErrorState } from "./error-state/error.state";

export interface IApplicationState {
    CLIENTS_STATE_NAME: IClientState;
    ERROR_STATE_NAME: IErrorState;
}