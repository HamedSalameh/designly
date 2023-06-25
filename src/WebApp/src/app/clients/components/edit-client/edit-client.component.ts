import { Component, EventEmitter, Output } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Observable, of } from 'rxjs';
import { ClientState } from 'src/app/state/client-state/client-state.state';
import { Client } from '../../models/client.model';
import { ClientsServiceService } from '../../services/clients-service.service';

@Component({
  selector: 'app-edit-client',
  templateUrl: './edit-client.component.html',
  styleUrls: ['./edit-client.component.scss'],
})
export class EditClientComponent {

  @Output() CloseClient: EventEmitter<any> = new EventEmitter();

  ClientInfo!: FormGroup;
  clientId;
  selectedClient$: Observable<Client | null> = of(null);
  selectedClient: Client | null = null;

  localizedSave = $localize`:@@Global.Save:Save`;
  localizedCancel = $localize`:@@Global.Cancel:Cancel`;

  localizedFirstName!: string;
  localizedFamilyName!: string;

  localizedPrimaryPhoneNumber!: string;
  localizedEmailAddress!: string;

  localizedCity!: string;
  localizedStreet!: string;
  localizedBuildingNumber!: string;
  localizedAddressLine1!: string;

  constructor(
    private clientsService: ClientsServiceService,
    private formBuilder: FormBuilder,
    private store: Store
  ) {
    this.clientId = this.store.select(ClientState.selectedClient);

    this.clientId.subscribe((clientId: any) => {
      if (clientId) {
        this.clientsService.getClient(clientId).subscribe((client: Client) => {
          this.selectedClient = client;
          this.createForm();
        });
      }
    });
  }

  ngOnInit(): void {
    // localize basic info labels
    this.localizedFirstName = $localize`:@@Global.BasicInfo.FirstName:FirstName`;
    this.localizedFamilyName = $localize`:@@Global.BasicInfo.FamilyName:FamilyName`;
    // localize address details labels
    this.localizedCity = $localize`:@@Global.AddressInfo.City:City`;
    this.localizedStreet = $localize`:@@Global.AddressInfo.Street:Street`;
    this.localizedBuildingNumber = $localize`:@@Global.AddressInfo.BuildingNumber:BuildingNumber`;
    this.localizedAddressLine1 = $localize`:@@Global.AddressInfo.AddressLine1:AddressLine1`;
    // localize contact details labels
    this.localizedPrimaryPhoneNumber = $localize`:@@Global.ContactInfo.PrimaryPhoneNumber:PrimaryPhoneNumber`;
    this.localizedEmailAddress = $localize`:@@Global.ContactInfo.EmailAddress:EmailAddress`;
  }

  createForm() {
    this.ClientInfo = this.formBuilder.group({
      BasicInfo: this.formBuilder.group({
        FirstName: [this.selectedClient?.FirstName],
        FamilyName: [this.selectedClient?.FamilyName],
        // CustomControl: [this.selectedClient?.FirstName]
      }),
      ContactDetails: this.formBuilder.group({
        PrimaryPhoneNumber: [
          this.selectedClient?.ContactDetails?.PrimaryPhoneNumber,
        ],
        EmailAddress: [this.selectedClient?.ContactDetails?.EmailAddress],
      }),
      AddressInfo: this.formBuilder.group({
        City: [this.selectedClient?.Address?.City],
        Street: [this.selectedClient?.Address?.Street],
        BuildingNumber: [this.selectedClient?.Address?.BuildingNumber],
        AddressLine1: [this.selectedClient?.Address?.AddressLines],
      }),
    });

    this.ClientInfo.valueChanges.subscribe((val) => {
      console.log(val);
    });
  }

  onCancel() {
    console.debug('[EditClientComponent] [onCancel]');
    this.CloseClient.emit();
  }

  onSave() {
    console.log('save');
  }

}
