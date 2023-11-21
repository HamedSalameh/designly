import { trigger, transition, style, animate } from '@angular/animations';
import { Component, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Subject, takeUntil } from 'rxjs';
import { Client } from '../../models/client.model';
import { DEVELOPMENT_TENANT_ID, NEW_CLIENT_ID } from 'src/app/shared/constants';
import {
  getClientRequest,
  addClientRequest,
  activateEditMode,
  selectClient,
  unselectClient,
  activateViewMode,
  updateClientRequest,
  deleteClientRequest,
  updateSelectedClientModel,
} from 'src/app/clients/client-state/clients.actions';
import {
  getApplicationState,
  getSelectedClientFromState,
  getViewModeFromState,
} from 'src/app/clients/client-state/clients.selectors';
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
  //clientId: string | null = null;
  activeClient: any;
  //activeClientId: any;
  private unsubscribe$: Subject<void> = new Subject();

  constructor(private store: Store<IApplicationState>) {
    this.handleEditMode();

    this.store.dispatch(activateViewMode());

    this.store
      .select(getViewModeFromState)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((viewMode: any) => {
        this.editMode = viewMode;
      });

    this.store
      .select(getSelectedClientFromState)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((client: any) => {
        this.activeClient = client;
      });
  }

  private handleEditMode(): void {
    this.store
      .select(getApplicationState)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(({ editMode }: any) => {
        this.editMode = editMode;
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  onSelectClient($event: string) {
    const selectedClientId = $event as string;

    if (selectedClientId) {
      this.store.dispatch(selectClient({ payload: selectedClientId }));
      this.store.dispatch(getClientRequest({ clientId: selectedClientId }));
    }
  }

  onAddClient() {
    if (this.activeClient) {
      this.activeClient = { ...this.activeClient, Id: NEW_CLIENT_ID };
    } else {
      this.activeClient = { Id: NEW_CLIENT_ID };
    }

    this.store.dispatch(updateSelectedClientModel({ payload: this.activeClient }));
    this.store.dispatch(activateEditMode({ payload: this.activeClient.Id }));
  }

  // View Client Event Handlers
  onClose(): void {
    console.debug('[ClientJacketComponent] [onClose]');
    this.store.dispatch(unselectClient());
    this.store.dispatch(activateViewMode());
  }

  onEdit(): void {
    console.debug('[ClientJacketComponent] [onEdit]');
    if (this.activeClient) {
      this.store.dispatch(activateEditMode({ payload: this.activeClient.Id }));
    }
  }

  onShare(): void {
    console.debug('[ClientJacketComponent] [onShare]', this.client);
  }

  onDelete(): void {
    if (this.activeClient)
      this.store.dispatch(
        deleteClientRequest({ clientId: this.activeClient.Id })
      );
  }

  // Edit client event handlers
  onCancelEditClient(): void {
    console.debug('[ClientJacketComponent] [onCancelEditClient]', this.client);

    this.store.dispatch(activateViewMode());

    if (this.activeClient?.Id === NEW_CLIENT_ID) {
      this.store.dispatch(unselectClient());
    }
  }

  onSaveEditClient(client: Client): void {
    client.Id === NEW_CLIENT_ID
      ? this.store.dispatch(addClientRequest({ draftClient: client }))
      : this.store.dispatch(updateClientRequest({ clientModel: client }));
  }
}
