import { trigger, transition, style, animate } from '@angular/animations';
import { Component, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Subject, takeUntil } from 'rxjs';
import { Client } from '../../models/client.model';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import {
  addClientRequest,
  activateEditMode,
  selectClient,
  unselectClient,
  activateViewMode,
  updateClientRequest,
  deleteClientRequest,
} from 'src/app/clients/client-state/clients.actions';
import {
  getClientById,
  getSingleClient,
  getViewModeFromState,
} from 'src/app/clients/client-state/clients.selectors';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { CreateDraftClient } from '../../factories/client.factory';

@Component({
    selector: 'app-clients-management',
    templateUrl: './clients-management.component.html',
    styleUrls: ['./clients-management.component.scss'],
    animations: [
        trigger('fadeInUp', [
            transition(':enter', [
                style({ opacity: 0, transform: 'translateY(40px)' }),
                animate('300ms ease-out', style({ opacity: 1, transform: 'translateY(0)' })),
            ]),
        ]),
        trigger('fadeInLeft', [
            transition(':enter', [
                style({ opacity: 0, transform: 'translateX(-20px)' }),
                animate('300ms ease-out', style({ opacity: 1, transform: 'translateX(0)' })),
            ]),
        ]),
    ],
    standalone: false
})
export class ClientsManagementComponent implements OnDestroy {
  client: any;
  editMode$: any;
  activeClient: any;  
  private unsubscribe$: Subject<void> = new Subject();

  constructor(private store: Store<IApplicationState>) {
    // The default view mode is activated when the component is initialized
    this.store.dispatch(activateViewMode());
    // The edit mode is activated when the user clicks on the edit button
    this.editMode$ = this.store.select(getViewModeFromState).pipe(
      takeUntil(this.unsubscribe$)
    );
    // The active client is retrieved from the store so it can be viewed or edited in the panel
    this.store
      .select(getSingleClient)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((client: any) => {
        this.activeClient = client;
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  onSelectClient($event: string) {
    const selectedClientId = $event as string;
    if (selectedClientId)
    {
      this.store.dispatch(selectClient({ clientId: selectedClientId }));
    }
  }

  onAddClient() {
    const draftClient = CreateDraftClient();

    this.store.dispatch(selectClient({ clientId: draftClient.Id }));
    this.store.dispatch(activateEditMode({ clientId: draftClient.Id }));
  }

  // View Client Event Handlers
  onClose(): void {
    this.store.dispatch(unselectClient());
    this.store.dispatch(activateViewMode());
  }

  onEdit(): void {
    if (this.activeClient) {
      this.store.dispatch(activateEditMode({ clientId: this.activeClient.Id }));
    }
  }

  onDelete(): void {
    if (this.activeClient)
      this.store.dispatch(
        deleteClientRequest({ clientId: this.activeClient.Id })
      );
  }

  // Edit client event handlers
  onCancelEditClient(): void {
    this.store.dispatch(activateViewMode());

    if (this.activeClient?.Id === NEW_CLIENT_ID) {
      this.store.dispatch(unselectClient());
    }
  }

  onSaveEditClient(client: Client): void {
    client.Id === NEW_CLIENT_ID
      ? this.store.dispatch(addClientRequest({ draftClientModel: client }))
      : this.store.dispatch(updateClientRequest({ clientModel: client }));
  }
}
