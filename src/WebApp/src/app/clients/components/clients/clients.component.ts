import { Component, EventEmitter, Output } from '@angular/core';
import { TableData } from '../../models/table-data.model';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { Store } from '@ngrx/store';
import { getClientsRequest } from '../../client-state/clients.actions';
import { getClients } from '../../client-state/clients.selectors';
import { Strings } from 'src/app/shared/strings';
import { ClientStrings } from '../../strings';

@Component({
    selector: 'app-clients',
    templateUrl: './clients.component.html',
    styleUrls: ['./clients.component.scss'],
    standalone: false
})
export class ClientsComponent {
  @Output() SelectRow: EventEmitter<string> = new EventEmitter<string>();
  @Output() AddClient: EventEmitter<void> = new EventEmitter<void>();

  columnsDefinition = [
    {
      ColumnHeader: Strings.FirstName,
      DataField: 'FirstName',
    },
    {
      ColumnHeader: Strings.FamilyName,
      DataField: 'FamilyName',
    },
    {
      ColumnHeader: Strings.City,
      DataField: 'City',
    },
    {
      ColumnHeader: Strings.Address,
      DataField: 'Address',
    },
    {
      ColumnHeader: Strings.PrimaryPhoneNumer,
      DataField: 'primaryPhoneNumber',
    },
    {
      ColumnHeader: Strings.EmailAddress,
      DataField: 'Email',
    },
  ];

  tableData: TableData[] = [];
  tableColumns: any[] = [];
  tableToolbarItems : any[] = [];

  constructor(private store: Store<IApplicationState>) {}

  ngOnInit(): void { 

    this.tableToolbarItems = [
      {
        // search
        text: ClientStrings.Search,
        tooltipText: ClientStrings.SearchTooltip,
        prefixIcon: 'e-search-icon',
        id: 'searchClientAction'
      }
      ,
      {
        text: ClientStrings.NewClient,
        tooltipText: 'Add',
        prefixIcon: 'e-plus',
        id: 'addClientAction',
      },
    ]

    this.store.dispatch(getClientsRequest());

    this.store.select(getClients).subscribe((clients) => {
      this.tableData = clients.map((client) => this.mapClientToTableData(client));
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
