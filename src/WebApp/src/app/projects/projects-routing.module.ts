import { inject, NgModule } from '@angular/core';
import { Routes, RouterModule, ResolveFn, provideRouter, withComponentInputBinding } from '@angular/router';
import { ProjectsHomeComponent } from './components/projects-home/projects-home.component';
import { AuthenticationGuard } from '../authentication/authentication.guard';
import { RouteConfiguration } from '../shared/models/route-configuration.model';
import { RouteFactory } from '../shared/providers/route-provider.factory';
import { Icons } from '../shared/icons';
import { select, Store } from '@ngrx/store';
import { Project } from './models/project.model';
import { of, switchMap, take } from 'rxjs';
import { getProjectById } from './projects-state/projects.selectors';
import { ManageProjectComponent } from './components/manage-project/manage-project.component';

export const projectRouteResolver: ResolveFn<any> = (route, state) => {
    // Try to get the project id from the route
    const projectId = route.paramMap.get('id') || '';
    
    // get the selected project by id from NGRX store
    const store = inject(Store<Project>);
    return store.pipe(
        select(getProjectById(projectId)),
        take(1),
        switchMap((selectedProject) => {
          if (selectedProject) {
            // Attach the project to route data
            route.data['breadcrumb'].label = selectedProject.Name ;
            return of(selectedProject);
          } else {
            console.warn(`Project with id ${projectId} not found`);
            return of(null);
          }
        })
      );

  };

const ModuleRoutes = new Map<string, RouteConfiguration>();

const selectedProject = RouteFactory.createRoute(
    ":id",
    "_selectedProjectPlaceholder_",
    "projects/:id",
    Icons.project
);

ModuleRoutes.set("selectedProject", selectedProject);

const routes: Routes = [
    { path: '', component: ProjectsHomeComponent },
    {
        path: ModuleRoutes.get('selectedProject')?.path,
        canActivate: [AuthenticationGuard],
        resolve: { project: projectRouteResolver },
        data: { breadcrumb: ModuleRoutes.get('selectedProject')?.breadcrumb },
        component: ManageProjectComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ProjectsRoutingModule { }