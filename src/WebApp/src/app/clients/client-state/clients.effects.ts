import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';
import {
  unselectClient,
  getClientRequest,
  updateClientRequest,
  activateViewMode,
  updateSelectedClientModel,
  addClientRequest,
} from './clients.actions';
import { ClientsService } from 'src/app/clients/services/clients.service';
import { Store } from '@ngrx/store';
import { raiseNetworkError } from 'src/app/shared/state/error-state/error.actions';

@Injectable()
export class ClientsEffects {
  constructor(
    private store: Store,
    private actions$: Actions,
    private clientsService: ClientsService
  ) {}

  getClient$ = createEffect(() =>
    this.actions$.pipe(
      tap((action) => console.log('getClient effect', action)),
      ofType(getClientRequest),
      mergeMap((action) => {
        return this.clientsService.getClient(action.clientId).pipe(
          map((client) => {
            console.log('getClient effect - OK', client);
            return updateSelectedClientModel({ payload: client });
          }),
          catchError((error) => {
            console.log('getClient effect - ERROR', error);
            this.store.dispatch(unselectClient());
            return of(raiseNetworkError({ payload: error }));
          })
        );
      })
    )
  );

  // add client effect
  addClient$ = createEffect(() =>
    this.actions$.pipe(
      ofType(addClientRequest),
      mergeMap((action) => {
        return this.clientsService.addClient(action.draftClient).pipe(
          map((client) => {
            console.debug('addClient effect - OK', client);
            // on effect success, dispatch an action to update the store
            this.store.dispatch(activateViewMode()); // Exit edit mode
            return updateSelectedClientModel({ payload: action.draftClient });
          }),
          catchError((error) => {
            console.debug('addClient effect - ERROR', error);
            return of(
              raiseNetworkError({
                payload: { message: 'Network error', originalError: error },
              })
            );
          })
        );
      })
    )
  );

  // update client effect
  updateClient$ = createEffect(() =>
    this.actions$.pipe(
      ofType(updateClientRequest),
      mergeMap((action) => {
        return this.clientsService.updateClient(action.clientModel).pipe(
          map((client) => {
            console.debug('updateClient effect - OK', client);
            // on effect success, dispatch an action to update the store
            this.store.dispatch(activateViewMode()); // Exit edit mode
            return updateSelectedClientModel({ payload: action.clientModel });
          }),
          catchError((error) => {
            console.debug('updateClient effect - ERROR', error);
            return of(
              raiseNetworkError({
                payload: { message: 'Network error', originalError: error },
              })
            );
          })
        );
      })
    )
  );
}
