import { Address } from "./address.model";
import { ContactDetails } from "./contact-details.models";

export interface Client {
  FirstName: string;
  FamilyName: string;
  TenantId: string;
  Address: Address;
  ContactDetails: ContactDetails;
}