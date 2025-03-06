import { Component } from '@angular/core';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { Store } from '@ngrx/store';
import { map, switchMap } from 'rxjs';
import { BuildProjectViewModelForProjectId } from '../../Builders/project-view-model.factory';
import { ProjectViewModel } from '../../models/ProjectViewModel';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { getAccountUsersFromState } from 'src/app/account/state/account.selectors';
import { Strings } from 'src/app/shared/strings';
import { getActiveProject } from '../../projects-state/projects.selectors';

@Component({
    selector: 'app-project-details-card',
    templateUrl: './project-details-card.component.html',
    styleUrls: ['./project-details-card.component.scss'],
    standalone: false
})
export class ProjectDetailsCardComponent {

  ProjectDetailsTitle!: string;
  ProjectDescription!: string;
  ProjectClient!: string;
  ProjectLead!: string;
  ProjectStartDate!: string;
  ProjectDeadline!: string;

  projectId!: string;

  activeProject?: ProjectViewModel;
  editMode: { [key: string]: boolean } = {};

  ProjectDetails!: FormGroup
  projectLeads: { label: string; value: string; }[] = [
  ];

  constructor(
    private formBuilder: FormBuilder,
    private store: Store<IApplicationState>
  ) { }

  private initializeEditMode() {
    if (this.activeProject) {
      Object.keys(this.activeProject).forEach(key => {
        this.editMode[key] = false;
      });
    }
  }

  toggleEditMode(key: string): void {
    this.editMode[key] = !this.editMode[key]; // Toggle edit mode
  }

  ngOnInit(): void {

    // Initialize strings (localized)
    this.ProjectDetailsTitle = Strings.ProjectDetails;
    this.ProjectDescription = Strings.ProjectDescription;
    this.ProjectClient = Strings.ProjectClient;
    this.ProjectLead = Strings.ProjectLead;
    this.ProjectStartDate = Strings.ProjectStartDate;
    this.ProjectDeadline = Strings.ProjectDeadline;

    this.store.select(getActiveProject).pipe(
      switchMap((activeProject) => BuildProjectViewModelForProjectId(this.store, activeProject?.Id!))
    ).subscribe((project) => {
      console.log(project);
      this.activeProject = project;
      this.initializeEditMode();
      this.initializeProjectLeadsList();
      this.createForm();

      this.store.dispatch
    });

  }

  // when we leave the component, we need to reset the edit mode and clear the form, clear the active project
  ngOnDestroy(): void {
    this.activeProject = undefined;
    this.editMode = {};
    this.ProjectDetails.reset();
  }

  private initializeProjectLeadsList() {
    this.store.select(getAccountUsersFromState).pipe(
      map((users) => {
        return users.map((user) => {
          return { label: `${user.firstName} ${user.lastName}`, value: user.id }
        });
      })
    ).subscribe((users) => {
      this.projectLeads = users;
    });

  }

  private getAssignedProjectLead(projectLeadId: string | undefined) {
    return this.projectLeads.find((lead) => lead.value === projectLeadId)?.value;
  }

  private createForm() {
    this.ProjectDetails = this.formBuilder.group({
      name: new FormControl(this.activeProject?.Name, [Validators.required]),
      projectLead: new FormControl(this.getAssignedProjectLead(this.activeProject?.ProjectLeadId), [Validators.required]),
      client: new FormControl(this.activeProject?.Client, [Validators.required]),
      startDate: new FormControl(this.activeProject?.StartDate),
      deadLine: new FormControl(this.activeProject?.Deadline),
      status: new FormControl(this.activeProject?.Status),
      description: new FormControl(this.activeProject?.Description)
    });

  }

  public onConfirmEdit(key: string) {
    this.toggleEditMode(key);
    console.debug('Form data', this.ProjectDetails.value);
  }

  public onCancelEdit(key: string) {
    this.toggleEditMode(key);
  }

  public onClientChange(clientId: any) {
    console.log(clientId);
  }
}
