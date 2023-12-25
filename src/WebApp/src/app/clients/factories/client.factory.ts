import { NEW_CLIENT_ID } from 'src/app/shared/constants';
import { Client } from '../models/client.model';

export function CreateDraftClient(): Client {
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
