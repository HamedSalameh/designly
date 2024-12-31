import { Component } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { combineLatest, Subject, takeUntil } from 'rxjs';
import { toastOptionsFactory } from '../providers/toast-options.factory';
import { IApplicationState } from '../state/app.state';
import { getNetworkError, getApplicationError, getUnknownError } from '../state/error-state/error.selectors';
import { Strings } from '../strings';

@Component({
  selector: 'app-global-notification-handler',
  templateUrl: './global-notification-handler.component.html',
  styleUrls: ['./global-notification-handler.component.scss']
})
export class GlobalNotificationHandlerComponent {

  networkErrorState;
  applicationErrorState;
  unknownErrorState;
  private unsubscribe$: Subject<void> = new Subject();

  constructor(
    private store: Store<IApplicationState>,
    private toastr: ToastrService
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
          const message = networkError.message;
          this.toastr.error(message, messageTitle, toastOptionsFactory());
        }
        if (applicationError) {
          console.log('applicationError', applicationError);
          const message = applicationError.message;
          this.toastr.error(message, messageTitle, toastOptionsFactory());
        }
        if (unknownError) {
          console.log('unknownError', unknownError);
          const message = unknownError.message;
          this.toastr.error(message, messageTitle, toastOptionsFactory());
        }
      });
  }

  ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

}
