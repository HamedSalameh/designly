import { IClientState } from "src/app/clients/client-state/clients.state";
import { IErrorState } from "./error-state/error.state";
import { IProjectsState } from "src/app/projects/projects-state/projects.state";
import { IAccountState } from "src/app/account/state/account.state";

export interface IApplicationState {
    CLIENTS_STATE_NAME: IClientState;
    PROJECTS_STATE_NAME: IProjectsState;
    ERROR_STATE_NAME: IErrorState;
    ACCOUNT_STATE_NAME: IAccountState;
}