import { createFeatureSelector, createSelector } from "@ngrx/store";
import { IProjectsState, PROJECTS_STATE_NAME, ProjectsAdapter } from "./projects.state";

export const ProjectsState = createFeatureSelector<IProjectsState>(PROJECTS_STATE_NAME);

export const projectsSelector = ProjectsAdapter.getSelectors();

export const getProjects = createSelector(ProjectsState, projectsSelector.selectAll);
