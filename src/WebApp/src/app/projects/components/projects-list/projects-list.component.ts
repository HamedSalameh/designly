import { Component, ContentChild, TemplateRef, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { Project } from '../../models/project.model';
import { getProjects } from '../../projects-state/projects.selectors';
import { getProjectsRequest } from '../../projects-state/projects.actions';
import { ProjectsStrings } from '../../strings';
import { Column } from '@syncfusion/ej2-angular-grids';
import { dataFormatProperty } from '@syncfusion/ej2/documenteditor';
import { ColumnDefinition } from '@syncfusion/ej2/diagrams';

@Component({
  selector: 'app-projects-list',
  templateUrl: './projects-list.component.html',
  styleUrls: ['./projects-list.component.scss']
})
export class ProjectsListComponent {


  @ViewChild('projectStatusTemplate', { static: true }) statusTemplate!: TemplateRef<any>;
  
  columnsDefinition = [
    {
      ColumnHeader: ProjectsStrings.ProjectName,
      DataField: 'Name',
    },
    {
      ColumnHeader: ProjectsStrings.ProjectDescription,
      DataField: 'Description',
    },
    {
      ColumnHeader: ProjectsStrings.ProjectStatus,
      DataField: 'Status',
      Template: 'custom'
    }
  ];

  tableData: Project[] = [];
  tableColumns: any[] = [];
  tableToolbarItems : any[] = [];

  // localizations
  projectName = ProjectsStrings.ProjectName;
  projectDescription = ProjectsStrings.ProjectDescription;
  projectLead = ProjectsStrings.ProjectLead;
  client = ProjectsStrings.Client;
  projectStatus = ProjectsStrings.ProjectStatus;

  constructor(private store: Store<IApplicationState>) {}

  ngOnInit(): void {

    this.store.dispatch(getProjectsRequest({
      searchProjectsRequest: {
        filters: []
      }
    }));

    this.store.select(getProjects).subscribe((projects) => {
      console.log('projects', projects);
      this.tableData = projects.map((project) => this.mapProjectToViewModel(project));
      this.tableColumns = this.columnsDefinition.map((column) => ({
        field: column.DataField,
        header: column.ColumnHeader,
        template: this.getColumnTemplate(column.DataField) //column.Template === 'custom' ? this.customTemplate : undefined
      }));
    });
  }

  private getColumnTemplate(dataField: any) {
    switch (dataField) {
      case 'Name':
        return undefined
      case 'Status':
        return this.statusTemplate;
      default:
        return undefined;
    }
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
