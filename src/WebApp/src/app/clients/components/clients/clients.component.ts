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
  columnsDefinition = [
    {
      ColumnHeader: $localize`:@@clients.Headers.FirstName:FirstName`,
      DataField: 'FirstName',
    },
    {
      ColumnHeader: $localize`:@@clients.Headers.Family:Family`,
      DataField: 'FamilyName',
    },
    {
      ColumnHeader: $localize`:@@clients.Headers.City:City`,
      DataField: 'City',
    },
    {
      ColumnHeader: $localize`:@@clients.Headers.Address:Address`,
      DataField: 'Address',
    },
    {
      ColumnHeader: $localize`:@@clients.Headers.PrimaryPhoneNumber:PrimaryPhoneNumber`,
      DataField: 'primaryPhoneNumber',
    },
    {
      ColumnHeader: $localize`:@@clients.Headers.Email:Email`,
      DataField: 'Email',
    },
  ];

  tableData: any[] = [];

  tableColumns: any[] = this.columnsDefinition.map((column) => ({
    field: column.DataField,
    header: column.ColumnHeader,
  }));

  constructor(
    private clientsService: ClientsServiceService,
    private store: Store
  ) {}

  ngOnInit(): void {
    this.clientsService
      .getClients()
      .pipe(
        tap((clients) => console.log(clients)),
        map((clients) =>
          clients.map((client) => this.mapClientToTableData(client))
        )
      )
      .subscribe((clients) => (this.tableData = clients));
  }

  onRowSelect($event: any): void {
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
    };
    this.store.dispatch(new SelectClient(client));
  }

  private mapClientToTableData(client: any) {
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
  }
}
