import { Injectable } from '@angular/core';
import { IApplicationError, IError } from '../types';
import { Store } from '@ngrx/store';
import { Strings } from '../strings';
import { raiseApplicationError } from '../state/error-state/error.actions';

@Injectable({
  providedIn: 'root'
})
export class ErrorTranslationService {

  constructor(private store: Store) { }

  handleError(error: IError) : void {
    var application : IApplicationError = {
      message: error.message,
      originalError: error.originalError,
      handled: false,
      type: 'ApplicationError'
    };
     this.store.dispatch(raiseApplicationError( application ));
  }
}
