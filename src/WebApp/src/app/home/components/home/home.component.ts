import { Component } from '@angular/core';
import { Subject, combineLatest, takeUntil } from 'rxjs';
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
  private unsubscribe$: Subject<void> = new Subject();

  constructor(
    private store: Store<IApplicationState>,
    private toastr: ToastrService,
    private errorTranslationService: ErrorTranslationService
  ) {
    this.networkErrorState = this.store.pipe(select(getNetworkError));
    this.applicationErrorState = this.store.pipe(select(getApplicationError));
    this.unknownErrorState = this.store.pipe(select(getUnknownError));
  }

  ngOnInit() {  
    combineLatest([
      this.networkErrorState,
      this.applicationErrorState,
      this.unknownErrorState,
    ])
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(([networkError, applicationError, unknownError]) => {
        const messageTitle = Strings.MessageTitle_Error;

        if (networkError) {
          console.log('networkError', networkError);
          const message = this.errorTranslationService.getErrorMessage(networkError);
          this.toastr.error(message, messageTitle, toastOptionsFactory());
        }
        if (applicationError) {
          console.log('applicationError', applicationError);
          const message = this.errorTranslationService.getErrorMessage(applicationError);
          this.toastr.error(message, messageTitle, toastOptionsFactory());
        }
        if (unknownError) {
          console.log('unknownError', unknownError);
          const message = this.errorTranslationService.getErrorMessage(unknownError);
          this.toastr.error(message, messageTitle, toastOptionsFactory());
        }
      });
  }

  ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

}
