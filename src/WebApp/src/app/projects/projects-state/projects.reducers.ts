import { createReducer, on } from "@ngrx/store";
import { InitialProjectsState, IProjectsState, ProjectsAdapter } from "./projects.state";
import { clearActiveProject, deleteRealestatePropertyRequestSuccess, getProjectsRequestSuccess, resetProjectsState, setActiveProject } from "./projects.actions";

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
            selectedProjectId: payload.Id,
            selectedProjectModel: payload
        }
    }),

    on(deleteRealestatePropertyRequestSuccess, (state) => {
        console.debug('Reducer triggered for deleteRealestatePropertyRequestSuccess');
    
        if (!state.selectedProjectId) return state; // No active project
    
        return ProjectsAdapter.updateOne(
            {
                id: state.selectedProjectId,
                changes: { PropertyId: undefined }, 
            },
            {
                ...state,
                selectedProjectModel: state.selectedProjectModel
                    ? { ...state.selectedProjectModel, PropertyId: undefined }
                    : undefined,
            }
        );
    }),

    on(clearActiveProject, (state) => {
        return {
            ...state,
            selectedProjectId: null,
            selectedProjectModel: null,
        }
    }),
    

    // Reset State
    on(resetProjectsState, (state) => {
        return ProjectsAdapter.removeAll({
            ...state, ...InitialProjectsState
        });
    })
)