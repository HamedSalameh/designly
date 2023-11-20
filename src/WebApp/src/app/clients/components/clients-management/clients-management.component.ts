import { trigger, transition, style, animate } from '@angular/animations';
import { Component, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Subject, of, switchMap, takeUntil, tap } from 'rxjs';
import { ClientsService } from '../../services/clients.service';
import { Client } from '../../models/client.model';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import { GetClientRequest, AddClientRequest, EditModeActivated, ClientSelectedEvent, UnselectClientEvent, ViewModeActivated, ClientUpdatedEvent } from 'src/app/clients/client-state/clients.actions';
import { getApplicationState } from 'src/app/clients/client-state/clients.selectors';
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
    private store: Store<IApplicationState>,
    private clientsService: ClientsService) {
    this.handleEditMode();

    this.store.dispatch(ViewModeActivated());
  }

  private handleEditMode(): void {
    this.store.select(getApplicationState)
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
      this.store.dispatch(ClientSelectedEvent({ payload: selectedClientId }));
      this.store.dispatch(GetClientRequest({ clientId: selectedClientId }))
    }
  }

  onAddClient() {
    this.clientId = NEW_CLIENT_ID;
    this.store.dispatch(UnselectClientEvent());
    this.store.dispatch(EditModeActivated({ payload: this.clientId }));
  }

  // View Client Event Handlers
  onClose(): void {
    this.clientId = null;
    this.store.dispatch(ViewModeActivated());
  }

  onEdit(): void {
    if (this.clientId) {
      this.store.dispatch(ClientSelectedEvent({ payload: this.clientId }));
      this.store.dispatch(EditModeActivated({ payload: this.clientId }));
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
        )
        .subscribe({
          next: (client: Client) => {
            this.onClose();
            this.store.dispatch(UnselectClientEvent());
          },
          error: (error: any) => {
            alert(error.message);
          },
        });
    }

    return;
  }

  // Edit client event handlers
  onCancelEditClient(): void {
    console.debug('[ClientJacketComponent] [onCancelEditClient]', this.client);
    
    this.store.dispatch(ViewModeActivated());
    //this.store.dispatch(new ViewMode());
    if (this.clientId === NEW_CLIENT_ID) this.clientId = '';
  }

  onSaveEditClient(client: Client): void {
    client.Id === NEW_CLIENT_ID
      ? this.store.dispatch(AddClientRequest({ draftClient: client }))
      : this.store.dispatch(ClientUpdatedEvent({ clientModel: client }));
  }
}
