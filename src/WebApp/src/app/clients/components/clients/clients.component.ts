import { trigger, transition, style, animate } from '@angular/animations';
import { Component, EventEmitter, Output } from '@angular/core';
import { Store } from '@ngxs/store';
import { map, tap } from 'rxjs';
import { ClientsService } from '../../services/clients.service';
import { TableData } from '../../models/table-data.model';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.scss'],
})
export class ClientsComponent {
  @Output() SelectRow: EventEmitter<string> = new EventEmitter<string>();
  @Output() AddClient: EventEmitter<void> = new EventEmitter<void>();

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

  tableData: TableData[] = [];

  tableColumns: any[] = [];

  constructor(private clientsService: ClientsService) {}

  ngOnInit(): void {
    this.clientsService
      .getClients()
      .pipe(
        map((clients) =>
          // Map the clients to the table data
          clients.map((client) => this.mapClientToTableData(client))
        )
      )
      .subscribe((clients) => {
        this.tableData = clients;
        // Create table columns based on the columns definition
        this.tableColumns = this.columnsDefinition.map((column) => ({
          field: column.DataField,
          header: column.ColumnHeader,
        }));
      });
  }

  onRowSelect(event: any): void {
    const selectedClientId: string = event?.Id || null;
    this.SelectRow.emit(selectedClientId);
  }

  onAddClient(): void {
    this.AddClient.emit();
  }

  private mapClientToTableData(client: any) {
    return {
      Id: client.Id,
      TenantId: client.TenantId,
      FirstName: client.FirstName,
      FamilyName: client.FamilyName,
      City: client.Address.City,
      Address: `${client.Address?.Street || ''}, ${client.Address?.BuildingNumber || ''}`,
      primaryPhoneNumber: client.ContactDetails.PrimaryPhoneNumber,
      Email: client.ContactDetails.EmailAddress,
    };
  }
}
