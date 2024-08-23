import { TenantId } from "../types/tenant-id.type";
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
    Client: string;
}
