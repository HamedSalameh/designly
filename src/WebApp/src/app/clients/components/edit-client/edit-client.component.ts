import {
  Component,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { EMPTY, Observable, catchError, of, switchMap, tap } from 'rxjs';
import { ClientState } from 'src/app/state/client-state/client-state.state';
import { Client } from '../../models/client.model';
import { ClientsService } from '../../services/clients.service';
import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import { ClientSelector, SelectedClientIdSelector } from 'src/app/state/client-state/x-selectors.state';
import { Store, select } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/models/application-state.interface.';
import { getClient } from 'src/app/state/client-state/x-actions.state';

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

  localizedSave = $localize`:@@Global.Save:Save`;
  localizedCancel = $localize`:@@Global.Cancel:Cancel`;
  localizedNewClient = $localize`:@@Global.NewClient:NewClient`;

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
    private clientsService: ClientsService,
    private formBuilder: FormBuilder,
    private store: Store<IApplicationState>
  ) {
      this.store.pipe(select(ClientSelector)).pipe(
      switchMap((selectedClientModel) => {
        if (!selectedClientModel || selectedClientModel.Id === NEW_CLIENT_ID) {
          this.selectedClient = this.createEmptyClient();
          this.createForm();
          return of(); // Return an observable that completes immediately
        }
        console.log('[EditClientComponent] creating form for existing client ... ');
        this.selectedClient = selectedClientModel;
        this.createForm();
        return EMPTY; // Return an empty observable or another observable if needed
      })
    ).subscribe();
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

  onCancel() {
    console.debug('[EditClientComponent] [onCancel]');
    this.CancelEditClient.emit();
  }

  onSave() {
    console.debug('[EditClientComponent] [onSave]');
    const client: Client = this.createClientFromForm();
    this.SaveEditClient.emit(client);
  }

  private createEmptyClient(): Client {
    return {
      Id: NEW_CLIENT_ID,
      FirstName: '',
      FamilyName: '',
      TenantId: '',
      ContactDetails: {
        PrimaryPhoneNumber: '',
        EmailAddress: '',
      },
      Address: {
        City: '',
        Street: '',
        BuildingNumber: '',
        AddressLines: [],
      },
    };
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
