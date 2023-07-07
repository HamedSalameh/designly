import { Address } from './address.model';
import { ContactDetails } from './contact-details.models';

export interface Client {
  Id: string;
  FirstName: string;
  FamilyName: string;

  // TenantId is the unique identifier of the tenant (company) that the client belongs to
  // In case of account with multiple users, all users will have the same TenantId
  TenantId: string;

  // Equavalent to CompanyId, BN or H.P. number (Israel)
  // Can be null for private clients (not companies)
  PrivateCompanyId?: string;

  // The client's address
  Address: Address;

  // The client's contact details
  ContactDetails: ContactDetails;
}
