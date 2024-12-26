import { Property } from "../models/property.model";

export function buildRealestatePropertyBuilder(rawData: any[]): Property[] {
    // Logging for debugging purposes, can be removed in production
    console.debug('Building RealestatePropertyBuilder');

    return rawData.map((data) => {
        // Calculate total area of all floors
        const totalArea = data.Floors.reduce((sum: number, floor: any) => sum + floor.Area, 0);

        return {
            Name: data.Name,
            PropertyType: data.PropertyType,
            Address: {
                City: data.Address.City,
                Street: data.Address.Street,
                BuildingNumber: data.Address.BuildingNumber,
                AddressLines: data.Address.AddressLines || [],
            },
            Floors: data.Floors.map((floor: any) => ({
                FloorNumber: floor.FloorNumber,
                Area: floor.Area,
            })),
            NumberOfFloors: data.Floors.length,
            TotalArea: totalArea,
            CreatedAt: data.CreatedAt,
            ModifiedAt: data.ModifiedAt,
            Id: data.Id,
            TenantId: {
                Id: data.TenantId?.Id || '', // Handle missing TenantId gracefully
            },
        };
    });
}