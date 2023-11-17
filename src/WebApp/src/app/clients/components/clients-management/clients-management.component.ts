import { trigger, transition, style, animate } from '@angular/animations';
import { Component, OnDestroy } from '@angular/core';
import { Store as xStore } from '@ngrx/store';
import { Subject, of, switchMap, takeUntil, tap } from 'rxjs';
import { ClientsService } from '../../services/clients.service';
import { Client } from '../../models/client.model';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import { getClient, AddClient, EditMode, SelectClient, UnselectClient, ViewMode, UpdateClient } from 'src/app/clients/client-state/x-actions.state';
import { getApplicationState } from 'src/app/clients/client-state/x-selectors.state';
import { IApplicationState } from 'src/app/shared/state/app.state';

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

  constructor(
    private xStore: xStore<IApplicationState>,
    private clientsService: ClientsService) {
    this.handleEditMode();

    this.xStore.dispatch(ViewMode());
  }

  private handleEditMode(): void {
    this.xStore.select(getApplicationState)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(({ editMode }: any) => {
        this.editMode = editMode;
      }
    );
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  onSelectClient($event: string) {
    const selectedClientId = $event as string;
    this.clientId = selectedClientId;

    if (selectedClientId) {
      this.xStore.dispatch(SelectClient({ payload: selectedClientId }));
      this.xStore.dispatch(getClient({ clientId: selectedClientId }))
    }
  }

  onAddClient() {
    this.clientId = NEW_CLIENT_ID;
    this.xStore.dispatch(UnselectClient());
    this.xStore.dispatch(EditMode({ payload: this.clientId }));
  }

  // View Client Event Handlers
  onClose(): void {
    this.clientId = null;
    this.xStore.dispatch(ViewMode());
  }

  onEdit(): void {
    if (this.clientId) {
      this.xStore.dispatch(SelectClient({ payload: this.clientId }));
      this.xStore.dispatch(EditMode({ payload: this.clientId }));
    }
  }

  onShare(): void {
    console.debug('[ClientJacketComponent] [onShare]', this.client);
  }

  onDelete(): void {
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
            this.xStore.dispatch(UnselectClient());
            //this.store.dispatch(new UnselectClient());
          },
          error: (error: any) => {
            alert(error.message);
            //this.store.dispatch(new AddApplicationError(error));
          },
        });
    }

    return;
  }

  // Edit client event handlers
  onCancelEditClient(): void {
    console.debug('[ClientJacketComponent] [onCancelEditClient]', this.client);
    
    this.xStore.dispatch(ViewMode());
    //this.store.dispatch(new ViewMode());
    if (this.clientId === NEW_CLIENT_ID) this.clientId = '';
  }

  onSaveEditClient(client: Client): void {

    // instead of the direct call to the service, we dispatch an action (effect) to the store
    client.Id === NEW_CLIENT_ID
      ? this.xStore.dispatch(AddClient({ draftClient: client }))
      : this.xStore.dispatch(UpdateClient({ clientModel: client }));

    console.debug('[ClientJacketComponent] [onSaveEditClient]', this.client);

    // const onComplete = (client: Client) => {
    //   console.debug('[ClientJacketComponent] [onSaveEditClient]', client);
    //   this.xStore.dispatch(ViewMode());
    //   //this.store.dispatch(new ViewMode());
    // };

    // const onError = (error: any) => {
    //   console.error('[ClientJacketComponent] [onSaveEditClient]', error);
    //   this.store.dispatch(new AddApplicationError(error));
    // };

    const saveObersable =
      client.Id === NEW_CLIENT_ID
        ? this.clientsService.addClient(client)
        : this.clientsService.updateClient(client);

    // saveObersable.subscribe({
    //   next: onComplete,
    //   error: onError,
    // });
  }
}
