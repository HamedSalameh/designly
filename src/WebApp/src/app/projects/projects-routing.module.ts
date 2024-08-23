import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProjectsHomeComponent } from './components/projects-home/projects-home.component';
import { ProjectDetailsComponent } from './components/project-details/project-details.component';

const routes: Routes = [
    { path: '', component: ProjectsHomeComponent },
    // route for project details (example : /projects/95b15a76-11dd-417e-a684-105f57a685ce)
    {
        path: ':id',
        component: ProjectDetailsComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ProjectsRoutingModule { }