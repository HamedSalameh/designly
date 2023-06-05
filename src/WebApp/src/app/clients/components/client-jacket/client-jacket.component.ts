import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable, of } from 'rxjs';
import { ClientState } from 'src/app/state/client-state/client-state.state';
import { Client } from '../../models/client.model';
import { ClientsServiceService } from '../../services/clients-service.service';
@Component({
  selector: 'app-client-jacket',
  templateUrl: './client-jacket.component.html',
  styleUrls: ['./client-jacket.component.scss']
})
export class ClientJacketComponent {

  clientId;
  selectedClient$: Observable<Client | null> = of(null);
  selectedClient: Client | null = null;

  @Input() client: Client | undefined;

  @Output() CloseClientJacket: EventEmitter<any> = new EventEmitter();

  constructor(
    private clientsService: ClientsServiceService,
    private store: Store
  ) {
    this.clientId = this.store.select(ClientState.selectedClient);

    this.clientId.subscribe((clientId) => {
      if (clientId) {
        this.clientsService.getClient(clientId).subscribe((client) => {
          this.client = client;
          this.selectedClient = client
            ? {
                ...client,
                Address: { City: client?.Address?.City },
                ContactDetails: {
                  PrimaryPhoneNumber:
                    client?.ContactDetails?.PrimaryPhoneNumber,
                  EmailAddress: client?.ContactDetails?.EmailAddress,
                },
              }
            : null;
        });
      }
    });
  }

  onClose() {
    this.client = undefined;
    this.CloseClientJacket.emit();
  }
}
