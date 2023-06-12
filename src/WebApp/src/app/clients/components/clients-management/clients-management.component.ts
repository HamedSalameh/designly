import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable, of } from 'rxjs';
import { SelectClient, UnselectClient } from 'src/app/state/client-state/client-state.actions';
import { ClientState } from 'src/app/state/client-state/client-state.state';

@Component({
  selector: 'app-clients-management',
  templateUrl: './clients-management.component.html',
  styleUrls: ['./clients-management.component.scss'],
})
export class ClientsManagementComponent {
  clientId: string | null = null;

  constructor(private store: Store) {
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
}
