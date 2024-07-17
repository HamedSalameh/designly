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

  tableData: Project[] = [];

  constructor(private store: Store<IApplicationState>) { }

  ngOnInit(): void {

    this.store.dispatch(getProjectsRequest({
      searchProjectsRequest: {
        filters: []
      }
    }));

    this.store.select(getProjects).subscribe((projects) => {
      console.log('projects', projects);
      this.tableData = projects.map((project) => this.mapProjectToViewModel(project));
    });
  }

  private mapProjectToViewModel(project: Project) {
    return {
      Id: project.Id,
      Name: project.Name,
      Description: project.Description,
      StartDate: project.StartDate,
      Deadline: project.Deadline,
      Status: project.Status,
      IsCompleted: project.IsCompleted,
      CreatedAt: project.CreatedAt,
      ModifiedAt: project.ModifiedAt,
      ProjectLeadId: project.ProjectLeadId,
      ClientId: project.ClientId,
      TenantId: project.TenantId
    };
  }
}
