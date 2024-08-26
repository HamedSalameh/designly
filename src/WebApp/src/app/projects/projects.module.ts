import { isDevMode, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectsHomeComponent } from './components/projects-home/projects-home.component';
import { ProjectsRoutingModule } from './projects-routing.module';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { PROJECTS_STATE_NAME } from './projects-state/projects.state';
import { ProjectsStateReducer } from './projects-state/projects.reducers';
import { ProjectsEffects } from './projects-state/projects.effects';
import { ProjectsListComponent } from './components/projects-list/projects-list.component';
import { SharedModule } from '../shared/shared.module';
import { ManageProjectComponent } from './components/manage-project/manage-project.component';
import { ProjectDetailsCardComponent } from './components/project-details-card/project-details-card.component';
import { ProjectWorkspaceComponent } from './components/project-workspace/project-workspace.component';

@NgModule({
  declarations: [
    ProjectsHomeComponent,
    ProjectsListComponent,
    ManageProjectComponent,
    ProjectDetailsCardComponent,
    ProjectWorkspaceComponent
  ],
  imports: [
    ProjectsRoutingModule,
    CommonModule,
    SharedModule,
    StoreModule.forFeature(PROJECTS_STATE_NAME, ProjectsStateReducer),
    EffectsModule.forFeature([ProjectsEffects]),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
      logOnly: !isDevMode(), // Restrict extension to log-only mode
      autoPause: true, // Pauses recording actions and state changes when the extension window is not open
      trace: false, //  If set to true, will include stack trace for every dispatched action, so you can see it in trace tab jumping directly to that part of code
      traceLimit: 75, // maximum stack trace frames to be stored (in case trace option was provided as true)
      connectOutsideZone: true // If set to true, the connection is established outside the Angular zone for better performance
    }),
  ]
})
export class ProjectsModule { }
