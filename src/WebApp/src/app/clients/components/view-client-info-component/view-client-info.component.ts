import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable, of } from 'rxjs';
import { ClientState } from 'src/app/state/client-state/client-state.state';
import { Client } from '../../models/client.model';
import { ClientsServiceService } from '../../services/clients-service.service';

@Component({
  selector: 'app-view-client-info',
  templateUrl: './view-client-info.component.html',
  styleUrls: ['./view-client-info.component.scss'],
})
export class ViewClientInfoComponent {
  @Output() CloseClient: EventEmitter<any> = new EventEmitter();
  @Output() EditClient: EventEmitter<any> = new EventEmitter();
  @Output() ShareClient: EventEmitter<any> = new EventEmitter();
  @Output() DeleteClient: EventEmitter<any> = new EventEmitter();

  clientId;
  selectedClient$: Observable<Client | null> = of(null);
  selectedClient: Client | null = null;

  constructor(
    private clientsService: ClientsServiceService,
    private store: Store
  ) {
    this.clientId = this.store.select(ClientState.selectedClient);

    this.clientId.subscribe((clientId: any) => {
      if (clientId) {
        this.clientsService.getClient(clientId).subscribe((client: Client) => {
          this.selectedClient = client;
        });
      }
    });
  }

  onClose() {
    console.debug('[ViewClientInfoComponent] [onClose] ', this.clientId);
    this.CloseClient.emit();
  }

  onEdit() {
    console.debug('[ViewClientInfoComponent] [onEdit] ', this.clientId);
    this.EditClient.emit();
  }

  onShare() {
    console.debug('[ViewClientInfoComponent] [onShare] ', this.clientId);
    this.ShareClient.emit();
  }

  onDelete() {
    console.debug('[ViewClientInfoComponent] [onDelete] ', this.clientId);
    this.DeleteClient.emit();
  }
}
