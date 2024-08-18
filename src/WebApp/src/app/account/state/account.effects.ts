import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { getAccountUsersRequest, getAccountUsersRequestSuccess } from './account.actions';
import { AccountService } from '../services/account.service';
import { Strings } from 'src/app/shared/strings';
import { HttpErrorHandlingService } from 'src/app/shared/services/error-handling.service';

@Injectable()
export class AccountEffects {
    
    constructor(
        private accountService: AccountService,
        private errorHandlingService: HttpErrorHandlingService,
        private actions$: Actions) {}

    // GetAccountUsers request effect
    getAccountUsers$ = createEffect(() => 
        this.actions$.pipe(
            ofType(getAccountUsersRequest),
            mergeMap((action) => {
                return this.accountService.getAccountUsers(action.payload).pipe(
                    map((accountUsers) => {
                        return getAccountUsersRequestSuccess({ payload: accountUsers });
                    }),
                    catchError((error) => {
                        error.message = Strings.UnableToLoadAccountUsers;
                        this.errorHandlingService.handleError(error);
                        return of();
                    })
                );
            })
        )
    );
}