import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { Store } from "@ngrx/store";
import { ToastrService } from "ngx-toastr";
import { catchError, map, mergeMap, of } from "rxjs";
import { HttpErrorHandlingService } from "src/app/shared/services/error-handling.service";
import { Strings } from "src/app/shared/strings";
import { getProjectsRequest, getProjectsRequestSuccess } from "./projects.actions";
import { ProjectsService } from "../services/projects.service";

@Injectable()
export class ProjectsEffects {
    
    constructor(
        private store: Store,
        private action$: Actions,
        private projectsService: ProjectsService,
        private toastr: ToastrService,
        private errorHandlingService: HttpErrorHandlingService
    ) {}

    // Get Projects List
    getProjectsList$ = createEffect(() => 
        this.action$.pipe(
            ofType(getProjectsRequest),
            mergeMap((action) => {
                return this.projectsService.getProjects(
                    action.searchProjectsRequest
                ).pipe(
                    map((projects) => {
                        return getProjectsRequestSuccess({ payload: projects });
                    }),
                    catchError((error) => {
                        error.message = Strings.UnableToLoadProjectsList;
                        this.errorHandlingService.handleError(error);
                        return of();
                    })
                );
            })
        )
    );
}