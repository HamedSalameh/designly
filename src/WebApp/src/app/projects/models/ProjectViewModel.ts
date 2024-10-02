import { ProjectStatus } from "./project-status.enum";
import { Property } from "./property.model";


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
    Property: Property;
}

export interface ProjectLeadViewModel {
    Id: string;
    FirstName: string;
    LastName: string;
}
