import { ErrorHandler, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
/*
  This class is used to catch errors that are not caught by the Angular error handling mechanism.
  The Angular error handling mechanism catches errors that occur in templates, directives,
  services, and dependency-injected objects during initialization and updates, and logs them to the console.
*/
export class GlobalErrorHandlerService implements ErrorHandler {

  constructor() { }

  handleError(error: any): void {
    
    console.log('GlobalErrorHandlerService: ', error);

  }  
}
