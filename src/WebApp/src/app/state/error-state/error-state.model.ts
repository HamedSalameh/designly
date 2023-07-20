import { IApplicationError, INetworkError, IServerError } from "src/app/shared/types";

export class ErrorStateModel {

    applicationError : IApplicationError | null = null;
    networkError : INetworkError | null = null;
    serverError : IServerError | null = null;

}