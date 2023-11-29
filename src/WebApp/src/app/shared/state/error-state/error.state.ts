import { IApplicationError, IError, INetworkError } from "src/app/shared/types";

export interface IErrorState {
    applicationError: IApplicationError | null;
    networkError: INetworkError | null;
    unknownError: IError | null;
}

export const initialErrorStateModel: IErrorState = {
    applicationError: null,
    networkError: null,
    unknownError: null
};