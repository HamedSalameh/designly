import { Member } from "src/app/account/models/member.model";
import { Client } from "src/app/clients/models/client.model";
import { Project } from "../models/project.model";
import { ProjectViewModel } from "../models/ProjectViewModel";
import { inject } from "@angular/core";
import { Store } from "@ngrx/store";
import { combineLatest, map, Observable, of, switchMap } from "rxjs";
import { getProjectById } from "../projects-state/projects.selectors";
import { getClientById } from "src/app/clients/client-state/clients.selectors";
import { getAccountUserById } from "src/app/account/state/account.selectors";

export function mapProjectToViewModel(project: Project, clients: Client[], users: Member[]): ProjectViewModel {
    const client = clients.find(client => client.Id === project.ClientId.Id);
    const projectLead = users.find(user => user.id === project.ProjectLeadId.Id);

    return {
        Id: project.Id,
        Name: project.Name,
        Description: project.Description,
        StartDate: project.StartDate,
        Deadline: project.Deadline,
        Status: project.Status,
        IsCompleted: project.IsCompleted,
        CreatedAt: project.CreatedAt,
        ModifiedAt: project.ModifiedAt,
        ProjectLead: `${projectLead?.firstName} ${projectLead?.lastName}`,
        Client: `${client?.FirstName} ${client?.FamilyName}`,
        Address: project.Address
    };
}

export function BuildProjectViewModelForProjectId(store: Store, ProjectId: string) : Observable<ProjectViewModel> {
    if (!ProjectId) {
        console.warn('ProjectId is not defined');
        return new Observable<ProjectViewModel>();
    }


    return store.select(getProjectById(ProjectId)).pipe(
        switchMap(project => {
            if (!project) {
                console.warn('Project not found');
                return of(); // Return an empty observable if no project is found
            }

            const clientId = project.ClientId.Id;
            const projectLeadId = project.ProjectLeadId.Id;

            return combineLatest([
                store.select(getClientById(clientId)),
                store.select(getAccountUserById(projectLeadId))
            ]).pipe(
                map(([client, projectLead]) => {
                    return {
                        Id: project.Id || '',
                        Name: project.Name || '',
                        Description: project.Description || '',
                        StartDate: project.StartDate,
                        Deadline: project.Deadline,
                        Status: project.Status!,
                        IsCompleted: project.IsCompleted,
                        CreatedAt: project.CreatedAt!,
                        ModifiedAt: project.ModifiedAt!,
                        ProjectLead: `${projectLead?.firstName} ${projectLead?.lastName}`,
                        Client: `${client?.FirstName} ${client?.FamilyName}`,
                        Address: project.Address
                    };
                })
            );
        })
    );
}