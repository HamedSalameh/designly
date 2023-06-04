import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { UnselectClient } from 'src/app/state/client-state/client-state.actions';
import { ClientState } from 'src/app/state/client-state/client-state.state';
import { Client } from '../../models/client.model';

@Component({
  selector: 'app-clients-management',
  templateUrl: './clients-management.component.html',
  styleUrls: ['./clients-management.component.scss'],
})
export class ClientsManagementComponent {
  selectedClient$: Observable<Client | null> | null;
  selectedClient: Client | null = null;

  constructor(private store: Store) {
    this.selectedClient$ = this.store.select(ClientState.selectedClient);

    this.selectedClient$.subscribe((client) => {
      this.selectedClient = client
        ? {
            ...client,
            Address: { City: client?.Address?.City },
            ContactDetails: {
              PrimaryPhoneNumber: client?.ContactDetails?.PrimaryPhoneNumber,
              EmailAddress: client?.ContactDetails?.EmailAddress,
            },
          }
        : null;
      console.log(
        `ClientsManagementComponent: ${this.selectedClient?.FirstName}`
      );
    });
  }

  onCloseClientJacket() {
    this.store.dispatch(new UnselectClient());
  }
}
