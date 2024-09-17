import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { Store } from '@ngrx/store';
import { map, switchMap } from 'rxjs';
import { BuildProjectViewModelForProjectId } from '../../Factories/project-view-model.factory';
import { ProjectViewModel } from '../../models/ProjectViewModel';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { getAccountUsersFromState } from 'src/app/account/state/account.selectors';

@Component({
  selector: 'app-project-details-card',
  templateUrl: './project-details-card.component.html',
  styleUrls: ['./project-details-card.component.scss']
})
export class ProjectDetailsCardComponent {

  projectIds = this.route.params.pipe(map((params) => params['id']));
  projectId!: string;

  activeProject?: ProjectViewModel;
  editMode: { [key: string]: boolean } = {};

  ProjectDetails!: FormGroup
  projectLeads : { label: string; value: string; }[] = [
  ];

  constructor(  
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private store: Store<IApplicationState>
  ) {}

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
    
    this.projectIds.pipe(
      switchMap((id) => BuildProjectViewModelForProjectId(this.store, id))
    ).subscribe((project) => {
      console.log(project);
      this.activeProject = project;
      this.initializeEditMode();
      this.initializeProjectLeadsList();
      this.createForm();
    });

  }

  private initializeProjectLeadsList(){
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
      description: new FormControl(this.activeProject?.Description),
      address: this.formBuilder.group({
        city: new FormControl(this.activeProject?.Address?.City) || 'City',
        street: new FormControl(this.activeProject?.Address?.Street),
        buildingNumber: new FormControl(this.activeProject?.Address?.BuildingNumber),
        addressLine1: new FormControl(this.activeProject?.Address?.AddressLines),
      })
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
