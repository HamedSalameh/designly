import { createReducer, on } from "@ngrx/store";
import { InitialProjectsState, IProjectsState, ProjectsAdapter } from "./projects.state";
import { getProjectsRequestSuccess, resetProjectsState, setActiveProject } from "./projects.actions";

export const ProjectsStateReducer = createReducer<IProjectsState>(
    InitialProjectsState,

    // GET PROJECTS
    on(getProjectsRequestSuccess, (state, { payload }) => {
        return ProjectsAdapter.setAll(payload, state);
    }),

    // Set Active Project
    on(setActiveProject, (state, { project: payload }) => {
        return {
            ...state,
            selectedProjectModel: payload
        }
    }),

    // Reset State
    on(resetProjectsState, (state) => {
        return ProjectsAdapter.removeAll({
            ...state, ...InitialProjectsState
        });
    })
)