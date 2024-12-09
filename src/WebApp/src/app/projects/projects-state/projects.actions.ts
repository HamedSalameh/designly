import { createAction, props } from "@ngrx/store";
import { SearchProjectsRequest } from "../models/search-pojects.request";

export const getProjectsRequest = createAction(
    '[ProjectsState] GetProjects',
    props<{ searchProjectsRequest: SearchProjectsRequest }>()
);
export const getProjectsRequestSuccess = createAction(
    '[ProjectsState] GetProjectsSuccess', 
    props<{ payload: any }>()
);
export const getProjectsRequestError = createAction(
    '[ProjectsState] GetProjectsError', 
    props<{ payload: any }>()
);