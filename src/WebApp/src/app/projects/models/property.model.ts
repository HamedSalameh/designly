import { Address } from "src/app/shared/models/address.model";
import { Floor } from "./floor.model";
import { PropertyType } from "./property-type.enum";


export interface Property {
    Name: string;
    PropertyType: PropertyType;
    Address: Address;
    Floors: Floor[];
    NumberOfFloors: number;
    TotalArea: number;
}
