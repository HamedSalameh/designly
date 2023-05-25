import { Component } from '@angular/core';
import { map, tap } from 'rxjs';
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
    { field: 'Street', header: 'Street' },
    { field: 'BuildingNumber', header: 'Building Number' },
    { field: 'Address', header: 'Address' },
    { field: 'primaryPhoneNumber', header: 'Primary Phone Number' },
    { field: 'Email', header: 'Email' },
  ];

  constructor(private clientsService: ClientsServiceService) {}

  ngOnInit(): void {
    this.clientsService
      .getClients()
      .pipe(
        tap((clients) => console.log(clients)),
        map((clients) =>
          clients.map((client) => {
            return {
              FirstName: client.FirstName,
              FamilyName: client.FamilyName,
              City: client.Address.City,
              Street: client.Address.Street,
              BuildingNumber: client.Address.BuildingNumber,
              Address: `${client.Address.City}, ${client.Address.Street} ${client.Address.BuildingNumber}`,
              primaryPhoneNumber: client.ContactDetails.PrimaryPhoneNumber,
              Email: client.ContactDetails.EmailAddress,
            };
          })
        )
      )
      .subscribe((clients) => (this.tableData = clients));
  }
}
