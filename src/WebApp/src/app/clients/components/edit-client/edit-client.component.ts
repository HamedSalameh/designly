import {
  Component,
  EventEmitter,
  OnInit,
  Output,
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of, switchMap, take, tap } from 'rxjs';
import { Client } from '../../models/client.model';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import { getSelectedClientIdFromState, getSingleClient } from 'src/app/clients/client-state/clients.selectors';
import { Store } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { CreateDraftClient } from '../../factories/client.factory';
import { getTenantId } from 'src/app/authentication/state/auth.selectors';
import { Strings } from 'src/app/shared/strings';
import { ClientStrings } from '../../strings';

@Component({
  selector: 'app-edit-client',
  templateUrl: './edit-client.component.html',
  styleUrls: ['./edit-client.component.scss'],
})
export class EditClientComponent implements OnInit {
  ClientInfo!: FormGroup;
  clientId: Observable<string | null> | undefined;
  selectedClient$: Observable<Client | null> = of(null);
  selectedClient: Client | null = null;
  newClientDraftId = NEW_CLIENT_ID;
  tenantId = '';

  localizedSave = Strings.Save;
  localizedCancel = Strings.Cancel;
  localizedNewClient = ClientStrings.NewClient;

  localizedFirstName!: string;
  localizedFamilyName!: string;

  localizedPrimaryPhoneNumber!: string;
  localizedEmailAddress!: string;

  localizedCity!: string;
  localizedStreet!: string;
  localizedBuildingNumber!: string;
  localizedAddressLine1!: string;

  validators = [Validators.required];

  @Output() CancelEditClient: EventEmitter<any> = new EventEmitter();
  @Output() SaveEditClient: EventEmitter<any> = new EventEmitter();

  constructor(
    private formBuilder: FormBuilder,
    private store: Store<IApplicationState>
  ) {
    this.store.select(getSelectedClientIdFromState).pipe(
      switchMap((selectedClientId) => {
        if (!selectedClientId || selectedClientId === NEW_CLIENT_ID) {
          this.selectedClient = CreateDraftClient();
          this.createForm();
          return of(null); // Return an observable that completes immediately
        }
        
        return this.store.select(getSingleClient).pipe(
          tap((client) => {
            this.selectedClient = client || null;
            this.createForm();
          })
        );
      }),
      take(1) // Ensure the observable completes after the first emission
    ).subscribe();
    
  }

  ngOnInit(): void {
    this.store.select(getTenantId).subscribe((tenantId) => {
      this.tenantId = tenantId || '';
    });
    // localize basic info labels
    this.localizedFirstName = Strings.FirstName;
    this.localizedFamilyName = Strings.FamilyName;
    // localize address details labels
    this.localizedCity = Strings.City;
    this.localizedStreet = Strings.Street;
    this.localizedBuildingNumber = Strings.BuildingNumber;
    this.localizedAddressLine1 = Strings.AddressLine1;
    // localize contact details labels
    this.localizedPrimaryPhoneNumber = Strings.PrimaryPhoneNumer;
    this.localizedEmailAddress = Strings.EmailAddress;
  }

  onCancel() {
    console.debug('[EditClientComponent] [onCancel]');
    this.CancelEditClient.emit();
  }

  onSave() {
    console.debug('[EditClientComponent] [onSave]');
    const client: Client = this.createClientFromForm();
    client.TenantId = this.tenantId;
    
    this.SaveEditClient.emit(client);
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
      return Strings.RequiredField;
    }
    return '';
  }
}
