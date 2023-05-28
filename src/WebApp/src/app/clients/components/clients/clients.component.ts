import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { map, tap } from 'rxjs';
import { SelectClient } from 'src/app/state/client-state/client-state.actions';
import { ClientsServiceService } from '../../services/clients-service.service';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.scss'],
})
export class ClientsComponent {
  
  tableData: any[] = [];

  tableColumns: any[] = [
    { field: 'FirstName', header: 'First Name' },
    { field: 'FamilyName', header: 'Family Name' },
    { field: 'City', header: 'City' },
    { field: 'Address', header: 'Address' },
    { field: 'primaryPhoneNumber', header: 'Primary Phone Number' },
    { field: 'Email', header: 'Email' },
  ];

  constructor(private clientsService: ClientsServiceService, private store: Store) {}

  ngOnInit(): void {
    this.clientsService
      .getClients()
      .pipe(
        tap((clients) => console.log(clients)),
        map((clients) =>
          clients.map((client) => {
            return {
              Id: client.Id,
              TenantId: client.TenantId,
              FirstName: client.FirstName,
              FamilyName: client.FamilyName,
              City: client.Address.City,
              Address: `${client.Address.Street}, ${client.Address.BuildingNumber}`,
              primaryPhoneNumber: client.ContactDetails.PrimaryPhoneNumber,
              Email: client.ContactDetails.EmailAddress,
            };
          })
        )
      )
      .subscribe((clients) => (this.tableData = clients));
  }

  onRowSelect($event: any) {

    console.log($event);
    const client = {
      Id: $event['Id'],
      FirstName: $event['FirstName'],
      FamilyName: $event['FamilyName'],
      TenantId: $event['TenantId'],
      Address: {
        City: $event['City'],
        Street: $event['Street'],
        BuildingNumber: $event['BuildingNumber'],
      },
      ContactDetails: {
        PrimaryPhoneNumber: $event['primaryPhoneNumber'],
        EmailAddress: $event['Email'],
      },
    }
    this.store.dispatch(new SelectClient( client ));
  }
}
