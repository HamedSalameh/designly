
export class SearchPropertiesRequest {
  id?: string | null;
  city?: string | null;
  street?: string | null;

  constructor(id: string | null, city: string | null, street: string | null) {
    this.id = id;
    this.city = city;
    this.street = street;
  }
}
