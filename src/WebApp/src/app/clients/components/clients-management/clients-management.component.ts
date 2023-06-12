import { trigger, transition, style, animate } from '@angular/animations';
import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { SelectClient, UnselectClient } from 'src/app/state/client-state/client-state.actions';

@Component({
  selector: 'app-clients-management',
  templateUrl: './clients-management.component.html',
  styleUrls: ['./clients-management.component.scss'],
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(40px)' }),
        animate('300ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ])
  ]
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
