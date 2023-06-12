import { Component, EventEmitter, Output } from '@angular/core';
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

  @Output() SelectRow: EventEmitter<string> = new EventEmitter<string>();

  columnsDefinition = [
    {
      ColumnHeader: $localize`:@@Global.BasicInfo.FirstName:FirstName`,
      DataField: 'FirstName',
    },
    {
      ColumnHeader: $localize`:@@Global.BasicInfo.FamilyName:FamilyName`,
      DataField: 'FamilyName',
    },
    {
      ColumnHeader: $localize`:@@Global.AddressInfo.City:City`,
      DataField: 'City',
    },
    {
      ColumnHeader: $localize`:@@Global.AddressInfo.Address:Address`,
      DataField: 'Address',
    },
    {
      ColumnHeader: $localize`:@@Global.ContactInfo.PrimaryPhoneNumber:PrimaryPhoneNumber`,
      DataField: 'primaryPhoneNumber',
    },
    {
      ColumnHeader: $localize`:@@Global.ContactInfo.EmailAddress:EmailAddress`,
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

  onRowSelect(event: any): void {
    // TODO: Instead of building the client from the table data, should we get the client from the store or local storage?
    // If we get the client from the store or local storage, we need to make sure that the client is up to date.
    // then getting the client from the store or local storage will be the using @Select decorator

    // const client = {
    //   Id: $event['Id'],
    //   FirstName: $event['FirstName'],
    //   FamilyName: $event['FamilyName'],
    //   TenantId: $event['TenantId'],
    //   Address: {
    //     City: $event['City'],
    //     Street: $event['Street'],
    //     BuildingNumber: $event['BuildingNumber'],
    //   },
    //   ContactDetails: {
    //     PrimaryPhoneNumber: $event['primaryPhoneNumber'],
    //     EmailAddress: $event['Email'],
    //   },
    // };
    const selectedClientId: string = event?.Id || null;
    this.SelectRow.emit(selectedClientId);
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
