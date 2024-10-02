import { PropertySpace } from "./property-space.model";

export interface Floor {
    FloorNumber: number;
    Area: number;
    Spaces: PropertySpace[];
}

