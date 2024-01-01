import { Injectable } from '@angular/core';
import { IApplicationError, IError } from '../types';
import { Store } from '@ngrx/store';
import { Strings } from '../strings';
import { raiseApplicationError } from '../state/error-state/error.actions';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ErrorTranslationService {

  constructor(private store: Store, private router: Router) { }

  handleError(error: IError) : void {

    var serverResponse = error.originalError?.status;
    // switch case for server response status
    switch (serverResponse) {
      case 400:
        error.message = Strings.BadRequest;
        break;
      case 401:
        error.message = Strings.Unauthorized;
        // redirect to login page
        this.router.navigateByUrl('/login');
        return;
      case 403:
        error.message = Strings.Forbidden;
        break;
      case 404:
        error.message = Strings.NotFound;
        break;
      case 500:
        error.message = Strings.InternalServerError;
        break;
      case 503:
        error.message = Strings.ServiceUnavailable;
        break;
      default:
        error.message = Strings.UnexpectedError;
        break;
    }

    var application : IApplicationError = {
      message: error.message,
      originalError: error.originalError,
      handled: false,
      type: 'ApplicationError'
    };
     this.store.dispatch(raiseApplicationError( application ));
  }
}
