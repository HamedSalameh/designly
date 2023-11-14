import { trigger, transition, style, animate } from '@angular/animations';
import { Component, OnDestroy } from '@angular/core';
import { Store } from '@ngxs/store';
import { select, Store as xStore } from '@ngrx/store';
import { Subject, of, switchMap, takeUntil, tap } from 'rxjs';
import {
  AddClient,
  EditMode,
  SelectClient,
  UnselectClient,
  ViewMode,
} from 'src/app/state/client-state/client-state.actions';
import { ClientState } from 'src/app/state/client-state/client-state.state';
import { ClientsService } from '../../services/clients.service';
import { Client } from '../../models/client.model';
import { AddApplicationError } from 'src/app/state/error-state/error-state.actions';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import { getClient, xAddClient, xEditMode, xSelectClient, xUnselectClient, xViewMode } from 'src/app/state/client-state/x-actions.state';
import { SelectedClientIdSelector, getApplicationState } from 'src/app/state/client-state/x-selectors.state';
import { IApplicationState } from 'src/app/shared/models/application-state.interface.';
import { IClientState } from 'src/app/shared/models/client-state.interface';

@Component({
  selector: 'app-clients-management',
  templateUrl: './clients-management.component.html',
  styleUrls: ['./clients-management.component.scss'],
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(40px)' }),
        animate(
          '300ms ease-out',
          style({ opacity: 1, transform: 'translateY(0)' })
        ),
      ]),
    ]),
    trigger('fadeInLeft', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateX(-20px)' }),
        animate(
          '300ms ease-out',
          style({ opacity: 1, transform: 'translateX(0)' })
        ),
      ]),
    ]),
  ],
})
export class ClientsManagementComponent implements OnDestroy {
  client: any;
  editMode: boolean = false;
  clientId: string | null = null;
  private unsubscribe$: Subject<void> = new Subject();

  constructor(private store: Store, 
    private xStore: xStore<IApplicationState>,
    private clientsService: ClientsService) {
    this.handleSelectedClient();
    this.handleEditMode();

    this.store.dispatch(new ViewMode());
  }

  private handleSelectedClient(): void {

    // updated logic: When selecting client, patch the application state


    // this.xStore.select(getSelectClient)
    //   .pipe(
    //     tap( () => console.log('getSelectedClient selector')),
    //     takeUntil(this.unsubscribe$),
    //     switchMap((clientId: any) => {
    //       return clientId
    //         ? clientId === NEW_CLIENT_ID
    //           ? of(NEW_CLIENT_ID)
    //           : this.clientsService.getClient(clientId)
    //         : of(null);
    //     })
    //   )
    //   .subscribe((clientFromServer: any) => {
    //     this.client = clientFromServer || null;
    //   });

    
    // this.store.select(ClientState.selectedClient)
    //   .pipe(
    //     takeUntil(this.unsubscribe$),
    //     switchMap((clientId: any) => {
    //       return clientId
    //         ? clientId === NEW_CLIENT_ID
    //           ? of(NEW_CLIENT_ID)
    //           : this.clientsService.getClient(clientId)
    //         : of(null);
    //     })
    //   )
    //   .subscribe((clientFromServer: any) => {
    //     this.client = clientFromServer || null;
    //   });
  }
  
  private handleEditMode(): void {
    this.xStore.select(getApplicationState)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(({ editMode }: any) => {
        this.editMode = editMode;
      }
    );

    // this.store.select(ClientState.applicationState)
    //   .pipe(takeUntil(this.unsubscribe$))
    //   .subscribe(({ editMode }: any) => {
    //     this.editMode = editMode;
    //   });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  onSelectClient($event: string) {
    const selectedClientId = $event as string;
    this.clientId = selectedClientId;

    if (selectedClientId) {
      this.xStore.dispatch(xSelectClient({ payload: selectedClientId }));
      this.xStore.dispatch(getClient({ clientId: selectedClientId }))
    }
  }

  onAddClient() {
    this.clientId = NEW_CLIENT_ID;

    this.xStore.dispatch(xAddClient());
    //this.store.dispatch(new AddClient());
  }

  // View Client Event Handlers
  onClose(): void {
    console.debug('[ClientJacketComponent] [onClose]', this.clientId);
    this.clientId = null;

    this.xStore.dispatch(xViewMode());
    //this.store.dispatch(new ViewMode());
  }

  onEdit(): void {
    console.debug('[ClientJacketComponent] [onEdit]', this.clientId);
    if (this.clientId) {

      this.xStore.dispatch(xEditMode({ payload: this.clientId }));
      //this.store.dispatch(new EditMode(this.client));
    }
  }

  onShare(): void {
    console.debug('[ClientJacketComponent] [onShare]', this.client);
  }

  onDelete(): void {
    console.debug('[ClientJacketComponent] [onDelete]', this.client);

    if (this.client) {
      this.clientsService
        .canDeleteClient(this.client.Id)
        .pipe(
          switchMap(() => this.clientsService.deleteClient(this.client.Id))
          // TODO: Should we catch error here?
        )
        .subscribe({
          next: (client: Client) => {
            this.onClose();
            // after successful delete, unselect the client and update the state
            this.xStore.dispatch(xUnselectClient());
            //this.store.dispatch(new UnselectClient());
          },
          error: (error: any) => {
            this.store.dispatch(new AddApplicationError(error));
          },
        });
    }

    return;
  }

  // Edit client event handlers
  onCancelEditClient(): void {
    console.debug('[ClientJacketComponent] [onCancelEditClient]', this.client);
    
    this.xStore.dispatch(xViewMode());
    //this.store.dispatch(new ViewMode());
    if (this.clientId === NEW_CLIENT_ID) this.clientId = '';
  }

  onSaveEditClient(client: Client): void {
    console.debug('[ClientJacketComponent] [onSaveEditClient]', this.client);

    const onComplete = (client: Client) => {
      console.debug('[ClientJacketComponent] [onSaveEditClient]', client);
      this.xStore.dispatch(xViewMode());
      //this.store.dispatch(new ViewMode());
    };

    const onError = (error: any) => {
      console.error('[ClientJacketComponent] [onSaveEditClient]', error);
      this.store.dispatch(new AddApplicationError(error));
    };

    const saveObersable =
      client.Id === NEW_CLIENT_ID
        ? this.clientsService.addClient(client)
        : this.clientsService.updateClient(client);

    saveObersable.subscribe({
      next: onComplete,
      error: onError,
    });
  }
}
