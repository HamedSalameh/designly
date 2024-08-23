import { ClientId } from "../types/client-id.type";
import { ProjectLeadId } from "../types/project-lead-it.type";
import { TenantId } from "../types/tenant-id.type";
import { ProjectStatus } from "./project-status.enum";

export interface Project {
    Id: string;
    Name: string;
    Description: string;

    ProjectLeadId: ProjectLeadId;
    ClientId: ClientId;
    TenantId: TenantId;

    StartDate?: Date;
    Deadline?: Date;
    CompletedAt?: Date;

    Status: ProjectStatus;
    IsCompleted: boolean
    CreatedAt: Date;
    ModifiedAt: Date;
}