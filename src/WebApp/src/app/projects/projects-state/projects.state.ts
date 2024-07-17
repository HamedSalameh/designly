import { createEntityAdapter, EntityState } from "@ngrx/entity";
import { Project } from "../models/project.model";

export const PROJECTS_STATE_NAME = 'projects';

export interface IProjectsState extends EntityState<Project> {
    selectedProjectId: string | null;
    selectedProjectModel: Project | null | undefined;
}

export const ProjectsAdapter = createEntityAdapter<Project>({
    selectId: (project: Project) => project.Id,
    sortComparer: false,
});

export const InitialProjectsState: IProjectsState = ProjectsAdapter.getInitialState({
    selectedProjectId: null,
    selectedProjectModel: null,
});