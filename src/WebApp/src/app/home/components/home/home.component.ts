import { Component } from '@angular/core';
import { Subject, combineLatest, takeUntil } from 'rxjs';
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
  

}
