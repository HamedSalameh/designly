import { ErrorHandler, Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { AddUnknownError } from 'src/app/state/error-state/error-state.actions';

@Injectable({
  providedIn: 'root'
})
/*
  This class is used to catch errors that are not caught by the Angular error handling mechanism.
  The Angular error handling mechanism catches errors that occur in templates, directives,
  services, and dependency-injected objects during initialization and updates, and logs them to the console.
*/
export class GlobalErrorHandlerService implements ErrorHandler {

  constructor(private store: Store) { }
  
  handleError(error: any): void {
    this.store.dispatch(new AddUnknownError(error));
    console.debug('[GlobalErrorHandler] [handleError] ', error);

  }
}
