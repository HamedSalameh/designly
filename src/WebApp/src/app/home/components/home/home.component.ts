import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
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
  errorState;

  constructor(private store: Store, toastMessageService: ToastMessageService,
    private errorTranslationService: ErrorTranslationService) {
    this.errorState = this.store.select(ErrorState.getNetworkError);

    this.errorState.subscribe((error: any) => {
      if (error) {
        const message = this.errorTranslationService.getTranslatedErrorMessage(error);
        const messageTitle = Strings.MessageTitle_Error;
        toastMessageService.showError(message, messageTitle);
      }
    });
  }
}
