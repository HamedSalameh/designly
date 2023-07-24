import { Injectable } from '@angular/core';
import { HttpResponseStatusCodes, INetworkError } from '../types';

@Injectable({
  providedIn: 'root'
})
export class ErrorTranslationService {

  constructor() { }

  getTranslatedErrorMessage(error: any): string {
    let message = '';

    switch (error.originalError?.status) {
      case HttpResponseStatusCodes.NOT_FOUND:
        message = $localize`:@@Errors.NotFound:The requested resource was not found.`;
        break;
      default:
        message = 'An error occurred while processing your request.';
        break;
    };

    return message;
  }
}
