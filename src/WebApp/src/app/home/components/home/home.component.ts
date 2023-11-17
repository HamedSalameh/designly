import { Component } from '@angular/core';
import { combineLatest } from 'rxjs';
import { ErrorTranslationService } from 'src/app/shared/services/error-translation.service';
import { ToastMessageService } from 'src/app/shared/services/toast-message-service.service';
import { Strings } from '../../../shared/strings';
import { Store, select } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { getNetworkError, getApplicationError, getUnknownError } from 'src/app/shared/state/error-state/error.selectors';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  networkErrorState;
  applicationErrorState;
  unknownErrorState;

  constructor(
    private store: Store<IApplicationState>,
    toastMessageService: ToastMessageService,
    private errorTranslationService: ErrorTranslationService
  ) {
    this.networkErrorState = this.store.pipe(select(getNetworkError));
    this.applicationErrorState = this.store.pipe(select(getApplicationError));
    this.unknownErrorState = this.store.pipe(select(getUnknownError));

    // subscribe to multuple observables
    let subscription = combineLatest([
      this.networkErrorState,
      this.applicationErrorState,
      this.unknownErrorState,
    ]).subscribe(([networkError, applicationError, unknownError]) => {
      const messageTitle = Strings.MessageTitle_Error;

      // Future enhancement: add a switch statement to handle different types of errors
      if (networkError) {
        const message = this.errorTranslationService.getTranslatedErrorMessage(networkError);
        toastMessageService.showError(message, messageTitle);
        return;
      }
      if (applicationError) {
        const message = this.errorTranslationService.getTranslatedErrorMessage(applicationError);
        toastMessageService.showError(message, messageTitle);
        return;
      }
      if (unknownError) {
        const message = this.errorTranslationService.getTranslatedErrorMessage(unknownError);
        toastMessageService.showError(message, messageTitle);
        return;
      }
    });
  }
}
