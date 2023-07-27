import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { Observable, of, tap } from 'rxjs';
import {
  InlineMessage,
  InlineMessageSeverity,
} from 'src/app/shared/components/inline-message/inline-message.component';
import { ErrorTranslationService } from 'src/app/shared/services/error-translation.service';
import { Strings } from 'src/app/shared/strings';
import { ClientState } from 'src/app/state/client-state/client-state.state';
import { Client } from '../../models/client.model';
import { ClientsServiceService } from '../../services/clients-service.service';

@Component({
  selector: 'app-edit-client',
  templateUrl: './edit-client.component.html',
  styleUrls: ['./edit-client.component.scss'],
})
export class EditClientComponent implements OnInit, OnDestroy {
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

  validators = [Validators.required];
  errorMessages: InlineMessage[] = [];

  constructor(
    private clientsService: ClientsServiceService,
    private formBuilder: FormBuilder,
    private store: Store,
    private ref: DynamicDialogRef,
    private errorTranslationService: ErrorTranslationService
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

    this.errorMessages = [];
  }

  onCancel() {
    console.debug('[EditClientComponent] [onCancel]');
    this.ref.close();
  }

  onSave() {
    console.debug('[EditClientComponent] [onSave]');

    const client: Client = this.createClientFromForm();

    // subscribe to the updateClient observable with pipe for error handling
    this.errorMessages = [];
    this.clientsService.updateClient(client)
    .subscribe({
      next: (client: Client) => {
        console.log(client);
        this.ref.close();
      },
      error: (error) => {
        const errorMessage =
          this.errorTranslationService.getTranslatedErrorMessage(error);

        this.errorMessages.push({
          severity: InlineMessageSeverity.ERROR,
          summary: Strings.MessageTitle_Error,
          detail: errorMessage,
        });
      }}
    );
  }

  ngOnDestroy(): void {
    console.debug('[EditClientComponent] [ngOnDestroy]');
    this.errorMessages = [];
  }

  private createForm() {
    this.ClientInfo = this.formBuilder.group({
      BasicInfo: this.formBuilder.group({
        FirstName: [this.selectedClient?.FirstName, [Validators.required]],
        FamilyName: [this.selectedClient?.FamilyName],
        // CustomControl: [this.selectedClient?.FirstName]
      }),
      ContactDetails: this.formBuilder.group({
        PrimaryPhoneNumber: [
          this.selectedClient?.ContactDetails?.PrimaryPhoneNumber,
          [Validators.required],
        ],
        EmailAddress: [this.selectedClient?.ContactDetails?.EmailAddress],
      }),
      AddressInfo: this.formBuilder.group({
        City: [this.selectedClient?.Address?.City, [Validators.required]],
        Street: [this.selectedClient?.Address?.Street],
        BuildingNumber: [this.selectedClient?.Address?.BuildingNumber],
        AddressLine1: [this.selectedClient?.Address?.AddressLines],
      }),
    });

    this.ClientInfo.valueChanges.subscribe((val) => {
      console.log(val);
    });
  }

  private createClientFromForm(): Client {
    return {
      Id: this.selectedClient?.Id!,
      FirstName: this.ClientInfo.get('BasicInfo.FirstName')?.value,
      FamilyName: this.ClientInfo.get('BasicInfo.FamilyName')?.value,
      TenantId: this.selectedClient?.TenantId!,
      ContactDetails: {
        PrimaryPhoneNumber: this.ClientInfo.get(
          'ContactDetails.PrimaryPhoneNumber'
        )?.value,
        EmailAddress: this.ClientInfo.get('ContactDetails.EmailAddress')?.value,
      },
      Address: {
        City: this.ClientInfo.get('AddressInfo.City')?.value,
        Street: this.ClientInfo.get('AddressInfo.Street')?.value,
        BuildingNumber: this.ClientInfo.get('AddressInfo.BuildingNumber')
          ?.value,
        AddressLines: this.ClientInfo.get('AddressInfo.AddressLine1')?.value,
      },
    };
  }

  getValidationMessage(formGroupName: string, formControlName: string) {
    const controlName = `${formGroupName}.${formControlName}`;
    const formControl = this.ClientInfo.get(controlName);
    if (formControl?.hasError('required')) {
      return $localize`:@@Global.Validation.Required:Required`;
    }
    return '';
  }
}
