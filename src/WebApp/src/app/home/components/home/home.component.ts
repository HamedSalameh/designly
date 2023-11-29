import { Component } from '@angular/core';
import { combineLatest } from 'rxjs';
import { ErrorTranslationService } from 'src/app/shared/services/error-translation.service';
import { Strings } from '../../../shared/strings';
import { Store, select } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { getNetworkError, getApplicationError, getUnknownError } from 'src/app/shared/state/error-state/error.selectors';
import { ToastrService } from 'ngx-toastr';
import { toastOptionsFactory } from 'src/app/shared/providers/toast-options.factory';

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
    private toastr: ToastrService,
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
        this.toastr.error(message, messageTitle, toastOptionsFactory());
        
        return;
      }
      if (applicationError) {
        const message = this.errorTranslationService.getTranslatedErrorMessage(applicationError);
        this.toastr.error(message, messageTitle, toastOptionsFactory());
        return;
      }
      if (unknownError) {
        const message = this.errorTranslationService.getTranslatedErrorMessage(unknownError);
        this.toastr.error(message, messageTitle, toastOptionsFactory());
        return;
      }
    });
  }
}
