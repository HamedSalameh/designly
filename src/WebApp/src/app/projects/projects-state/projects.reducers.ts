import { createReducer, on } from "@ngrx/store";
import { InitialProjectsState, IProjectsState, ProjectsAdapter } from "./projects.state";
import { getProjectsRequestSuccess } from "./projects.actions";

export const ProjectsStateReducer = createReducer<IProjectsState>(
    InitialProjectsState,

    // GET PROJECTS
    on(getProjectsRequestSuccess, (state, { payload }) => {
        return ProjectsAdapter.setAll(payload, state);
    }),
)