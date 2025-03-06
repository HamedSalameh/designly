import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { Project } from '../../models/project.model';
import { getProjects } from '../../projects-state/projects.selectors';
import { getProjectsRequest } from '../../projects-state/projects.actions';
import { ProjectsStrings } from '../../strings';
import { getClientsRequest } from 'src/app/clients/client-state/clients.actions';
import { combineLatest, map } from 'rxjs';
import { getClients } from 'src/app/clients/client-state/clients.selectors';
import { ProjectViewModel } from '../../models/ProjectViewModel';
import { getAccountUsersRequest } from 'src/app/account/state/account.actions';
import { getAccountUsersFromState } from 'src/app/account/state/account.selectors';
import { mapProjectToViewModel } from '../../Builders/project-view-model.factory';

@Component({
    selector: 'app-projects-list',
    templateUrl: './projects-list.component.html',
    styleUrls: ['./projects-list.component.scss'],
    standalone: false
})
export class ProjectsListComponent {


  @ViewChild('projectStatusTemplate', { static: true }) statusTemplate!: TemplateRef<any>;
  @ViewChild('projectNameTemplate', { static: true }) nameTemplate!: TemplateRef<any>;

  columnsDefinition = [
    {
      ColumnHeader: ProjectsStrings.ProjectName,
      DataField: 'Name'
    },
    {
      ColumnHeader: ProjectsStrings.ProjectDescription,
      DataField: 'Description',
    },
    {
      ColumnHeader: ProjectsStrings.ProjectLead,
      DataField: 'ProjectLead',
    },
    {
      ColumnHeader: ProjectsStrings.Client,
      DataField: 'Client',
    },
    {
      ColumnHeader: ProjectsStrings.ProjectStatus,
      DataField: 'Status'
    }
  ];

  tableData: ProjectViewModel[] = [];
  tableColumns: any[] = [];
  tableToolbarItems: any[] = [];

  // localizations
  projectName = ProjectsStrings.ProjectName;
  projectDescription = ProjectsStrings.ProjectDescription;
  projectLead = ProjectsStrings.ProjectLead;
  client = ProjectsStrings.Client;
  projectStatus = ProjectsStrings.ProjectStatus;

  constructor(private store: Store<IApplicationState>) { }

  ngOnInit(): void {

    this.store.dispatch(getProjectsRequest({
      searchProjectsRequest: {
        filters: []
      }
    }));

    this.store.dispatch(getClientsRequest());

    this.store.dispatch(getAccountUsersRequest({
      searchUsersRequest: {
        filters: []
      }
    }));

    combineLatest([
      this.store.select(getProjects),
      this.store.select(getClients),
      this.store.select(getAccountUsersFromState)
    ]).pipe(
      map(([projects, clients, users]) => {
        return projects.map(project => mapProjectToViewModel(project, clients, users));
      })
    ).subscribe((projects) => {
      console.log('projects', projects);
      this.tableData = projects;
      this.tableColumns = this.columnsDefinition.map((column) => ({
        field: column.DataField,
        header: column.ColumnHeader,
        template: this.getColumnTemplate(column.DataField)
      }));
    });
  }

  private getColumnTemplate(dataField: any) {
    switch (dataField) {
      case 'Name':
        return this.nameTemplate;
      case 'Status':
        return this.statusTemplate;
      default:
        return undefined;
    }
  }
}