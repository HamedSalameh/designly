import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable, of, switchMap } from 'rxjs';
import { ClientState } from 'src/app/state/client-state/client-state.state';
import { Client } from '../../models/client.model';
import { ClientsService } from '../../services/clients.service';

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

  selectedClient$: Observable<Client | null> = of(null);

  constructor(
    private clientsService: ClientsService,
    private store: Store
  ) {
    this.selectedClient$ = this.store.select(ClientState.selectedClient)
    .pipe(
      switchMap((clientId: any) => {
        if (clientId) {
          return this.clientsService.getClient(clientId);
        }
        return of(null);
      })
    )
  }

  onClose() {
    this.CloseClient.emit();
  }

  onEdit() {
    this.EditClient.emit();
  }

  onShare() {
    this.ShareClient.emit();
  }

  onDelete() {
    this.DeleteClient.emit();
  }
}
