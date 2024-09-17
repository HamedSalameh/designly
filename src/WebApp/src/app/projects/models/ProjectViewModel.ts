import { Address } from "src/app/shared/models/address.model";
import { ProjectStatus } from "./project-status.enum";


export interface ProjectViewModel {
    Id: string;
    Name: string;
    Description: string;
    StartDate?: Date;
    Deadline?: Date;
    Status: ProjectStatus;
    IsCompleted?: boolean;
    CreatedAt: Date;
    ModifiedAt: Date;
    ProjectLead: string;
    ProjectLeadId: string;
    Client: string;
    Address: Address;
}

export interface ProjectLeadViewModel {
    Id: string;
    FirstName: string;
    LastName: string;
}
