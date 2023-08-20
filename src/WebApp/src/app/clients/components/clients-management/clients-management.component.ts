import { trigger, transition, style, animate } from '@angular/animations';
import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { Subject, of, switchMap, takeUntil } from 'rxjs';
import {
  AddClient,
  EditMode,
  SelectClient,
  UnselectClient,
  ViewMode,
} from 'src/app/state/client-state/client-state.actions';
import { ClientState } from 'src/app/state/client-state/client-state.state';
import { ClientsServiceService } from '../../services/clients-service.service';
import { Client } from '../../models/client.model';
import { AddApplicationError } from 'src/app/state/error-state/error-state.actions';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';

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
export class ClientsManagementComponent {
  client: any;
  editMode: boolean = false;
  clientId: string | null = null;
  private unsubscribe$: Subject<void> = new Subject();

  constructor(
    private store: Store,
    private clientsService: ClientsServiceService
  ) {
    this.store
      .select(ClientState.selectedClient)
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap((clientId: any) => {
          return clientId
            ? clientId === NEW_CLIENT_ID
              ? of(NEW_CLIENT_ID) // New client
              : this.clientsService.getClient(clientId) // Existing client
            : of(null); // No client was selected
        })
      )
      .subscribe((clientFromServer: any) => {
        if (clientFromServer) {
          this.client = clientFromServer;
        } else {
          this.client = null;
        }
      });

    this.store
      .select(ClientState.applicationState)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((editMode: any) => {
        this.editMode = editMode.editMode;
      });

    this.store.dispatch(new ViewMode());
  }

  onSelectClient($event: string) {
    const selectedClientId = $event as string;
    this.clientId = selectedClientId;
    this.store.dispatch(new SelectClient(selectedClientId));
  }

  onCloseClientJacket() {
    this.clientId = null;
    this.store.dispatch(new UnselectClient());
  }

  onAddClient() {
    this.clientId = NEW_CLIENT_ID;
    this.store.dispatch(new AddClient());
  }

  // View Client Event Handlers
  onClose(): void {
    console.debug('[ClientJacketComponent] [onClose]', this.clientId);
    this.clientId = null;
    this.store.dispatch(new ViewMode());
  }

  onEdit(): void {
    console.debug('[ClientJacketComponent] [onEdit]', this.client);
    if (this.client) {
      this.store.dispatch(new EditMode(this.client));
    }
  }

  onShare(): void {
    console.debug('[ClientJacketComponent] [onShare]', this.client);
  }

  onDelete(): void {
    console.debug('[ClientJacketComponent] [onDelete]', this.client);

    if (this.client) {
      this.clientsService
        .canDeleteClient(this.client)
        .pipe(
          switchMap(() => this.clientsService.deleteClient(this.client))
          // TODO: Should we catch error here?
        )
        .subscribe({
          next: (client: Client) => {
            this.onClose();
          },
          error: (error: any) => {
            this.store.dispatch(new AddApplicationError(error));
          },
        });
    }

    return;
  }

  // Edit client event handlers
  // EditClient component
  onCancelEditClient(): void {
    console.debug('[ClientJacketComponent] [onCancelEditClient]', this.client);
    this.store.dispatch(new ViewMode());
    if (this.clientId === NEW_CLIENT_ID) this.clientId = '';
  }

  onSaveEditClient(client: Client): void {
    console.debug('[ClientJacketComponent] [onSaveEditClient]', this.client);

    const onComplete = (client: Client) => {
      console.debug('[ClientJacketComponent] [onSaveEditClient]', client);
      this.store.dispatch(new ViewMode());
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
