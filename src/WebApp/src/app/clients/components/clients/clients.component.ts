import { trigger, transition, style, animate } from '@angular/animations';
import { Component, EventEmitter, Output } from '@angular/core';
import { Store } from '@ngxs/store';
import { map, tap } from 'rxjs';
import { ClientsServiceService } from '../../services/clients-service.service';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.scss']
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
