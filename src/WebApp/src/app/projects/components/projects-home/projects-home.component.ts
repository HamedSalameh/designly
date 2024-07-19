import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { getProjectsRequest } from '../../projects-state/projects.actions';
import { getProjects } from '../../projects-state/projects.selectors';
import { Project } from '../../models/project.model';

@Component({
  selector: 'app-projects-home',
  templateUrl: './projects-home.component.html',
  styleUrls: ['./projects-home.component.scss']
})
export class ProjectsHomeComponent {


}
