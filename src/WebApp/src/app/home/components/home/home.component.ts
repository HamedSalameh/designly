import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { combineLatest } from 'rxjs';
import { ErrorTranslationService } from 'src/app/shared/services/error-translation.service';
import { ToastMessageService } from 'src/app/shared/services/toast-message-service.service';
import { ErrorState } from 'src/app/state/error-state/error-state.state';
import { Strings } from '../../../shared/strings';

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
    private store: Store,
    toastMessageService: ToastMessageService,
    private errorTranslationService: ErrorTranslationService
  ) {
    this.networkErrorState = this.store.select(ErrorState.getNetworkError);
    this.applicationErrorState = this.store.select(ErrorState.getApplicationError
    );
    this.unknownErrorState = this.store.select(ErrorState.getUnknownError);

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
