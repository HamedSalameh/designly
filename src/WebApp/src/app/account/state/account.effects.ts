import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap, switchMap, take } from 'rxjs/operators';
import { getAccountUsersRequest, getAccountUsersRequestSuccess } from './account.actions';
import { AccountService } from '../services/account.service';
import { Strings } from 'src/app/shared/strings';
import { HttpErrorHandlingService } from 'src/app/shared/services/error-handling.service';
import { Store } from '@ngrx/store';
import { getTenantId } from 'src/app/authentication/state/auth.selectors';

@Injectable()
export class AccountEffects {
    
    constructor(
        private accountService: AccountService,
        private errorHandlingService: HttpErrorHandlingService,
        private actions$: Actions,
        private store: Store) {}

    // GetAccountUsers request effect
    getAccountUsers$ = createEffect(() => 
        this.actions$.pipe(
            ofType(getAccountUsersRequest),
            mergeMap((action) => 
                this.store.select(getTenantId).pipe(
                    take(1),
                    mergeMap((accountId) => {
                        if (!accountId) {
                            return of();
                        }
                        return this.accountService.getAccountUsers(accountId, action.searchUsersRequest).pipe(
                            map((accountUsers) => getAccountUsersRequestSuccess({ payload: accountUsers })),
                            catchError((error) => {
                                error.message = Strings.UnableToLoadAccountUsers;
                                this.errorHandlingService.handleError(error);
                                return of();
                            })
                        );
                    })
                )
            )
        )
    );
}