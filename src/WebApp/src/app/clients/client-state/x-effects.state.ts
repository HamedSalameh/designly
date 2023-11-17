import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';
import { getClient, getClientFailure, getClientSucess } from './x-actions.state';
import { ClientsService } from 'src/app/clients/services/clients.service';

@Injectable()
export class ClientsEffects {
    constructor(private actions$: Actions, private clientsService: ClientsService) {}

    getClient$ = createEffect( () => 
        this.actions$.pipe(
            tap((action) => console.log('getClient effect', action)),
            ofType(getClient),
            mergeMap((action) => {
                return this.clientsService
                .getClient(action.clientId )
                .pipe(
                    map( (client) => getClientSucess({ payload: client })),
                    catchError((error) => of(getClientFailure({ payload: error }))))
                })
        )
    );
}
