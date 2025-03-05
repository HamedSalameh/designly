import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { ToastrService } from "ngx-toastr";
import { catchError, EMPTY, map, mergeMap, of } from "rxjs";
import { HttpErrorHandlingService } from "src/app/shared/services/error-handling.service";
import { Strings } from "src/app/shared/strings";
import { deleteRealestatePropertyRequest, deleteRealestatePropertyRequestSuccess, getProjectsRequest, getProjectsRequestSuccess } from "./projects.actions";
import { ProjectsService } from "../services/projects.service";
import { RealestatePropertyService } from "../services/realestate-property.service";
import { RealestatePropertyStrings } from "../real-estate-property-strings";

@Injectable()
export class ProjectsEffects {

    constructor(
        private action$: Actions,
        private projectsService: ProjectsService,
        private realestatePropertyService: RealestatePropertyService,
        private toastr: ToastrService,
        private errorHandlingService: HttpErrorHandlingService
    ) { }

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

    deleteRealestatePropertyRequest$ = createEffect(() =>
        this.action$.pipe(
            ofType(deleteRealestatePropertyRequest),
            mergeMap((action) =>
                this.realestatePropertyService.deleteProperty(action.propertyId).pipe(
                    map(() => {
                        console.debug('[ProjectsEffects] [deleteRealestatePropertyRequest] - OK');
                        this.toastr.success(RealestatePropertyStrings.DeletePropertySuccess);
                        return deleteRealestatePropertyRequestSuccess(); // 
                    }),
                    catchError((error) => {
                        error.message = RealestatePropertyStrings.UnableToDeleteRealestateProperty;
                        this.errorHandlingService.handleError(error);
                        return EMPTY;
                    })
                )
            )
        )
    );
    

}