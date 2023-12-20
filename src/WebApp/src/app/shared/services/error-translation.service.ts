import { Injectable } from '@angular/core';
import { HttpResponseStatusCodes, IError, INetworkError } from '../types';
@Injectable({
  providedIn: 'root',
})
export class ErrorTranslationService {
  constructor() {}

  getErrorMessage(error: IError): string {
    if (error.message) {   
      return error.message;
    }

    return this.getErrorMessageByErrorStatus(error);
  }

  private getErrorMessageByErrorStatus(error: any): string {
    let message = '';

    switch (error.originalError?.status) {
      case HttpResponseStatusCodes.NOT_FOUND:
        message = $localize`:@@Errors.NotFound:The requested resource was not found.`;
        break;
      case HttpResponseStatusCodes.BAD_REQUEST:
        message = $localize`:@@Errors.BadRequest:The request was invalid.`;
        break;
      case HttpResponseStatusCodes.UNAUTHORIZED:
        message = $localize`:@@Errors.Unauthorized:You are not authorized to perform this action.`;
        break;
      case HttpResponseStatusCodes.FORBIDDEN:
        message = $localize`:@@Errors.Forbidden:You are not authorized to perform this action.`;
        break;
      case HttpResponseStatusCodes.INTERNAL_SERVER_ERROR:
        message = $localize`:@@Errors.InternalServerError:An error occurred while processing your request.`;
        break;
      default:
        message = 'An error occurred while processing your request.';
        break;
    }

    return message;
  }
}
