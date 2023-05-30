import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ClientState } from 'src/app/state/client-state/client-state.state';
import { Client } from '../../models/client.model';

@Component({
  selector: 'app-clients-management',
  templateUrl: './clients-management.component.html',
  styleUrls: ['./clients-management.component.scss']
})
export class ClientsManagementComponent {

  selectedClient$: Observable<any> | null;
  selectedClient: Client | null = null;

  constructor(private store: Store) {
    this.selectedClient$ = this.store.select(ClientState.selectedClient);
    
    this.selectedClient$?.subscribe( (client) => {
      console.log('client-jacket.component.ts: constructor: client: ', client);
      if (client) {
        this.selectedClient = {
          Id: client?.Id,
          FirstName: client?.FirstName,
          FamilyName: client?.FamilyName,
          TenantId: client?.TenantId,
          Address: {
            City: client?.Address?.City,
          },
          ContactDetails: {
            PrimaryPhoneNumber: client?.ContactDetails?.PrimaryPhoneNumber,
            EmailAddress: client?.ContactDetails?.Email,
          },
        }
      }
    });
  }

}
