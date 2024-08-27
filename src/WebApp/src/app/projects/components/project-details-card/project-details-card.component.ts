import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { Store } from '@ngrx/store';
import { map, switchMap } from 'rxjs';
import { BuildProjectViewModelForProjectId } from '../../Factories/project-view-model.factory';
import { ProjectViewModel } from '../../models/ProjectViewModel';

@Component({
  selector: 'app-project-details-card',
  templateUrl: './project-details-card.component.html',
  styleUrls: ['./project-details-card.component.scss']
})
export class ProjectDetailsCardComponent {

  constructor(private route: ActivatedRoute,
    private store: Store<IApplicationState>
  ) { }

  projetcId$ = this.route.params.pipe(map((params) => params['id']));
  projectId!: string;

  activeProject?: ProjectViewModel;

  ngOnInit(): void {

    this.projetcId$.pipe(
      switchMap((id) => BuildProjectViewModelForProjectId(this.store, id))
    ).subscribe((project) => {
      console.log(project);
      this.activeProject = project;
    });
  }
}
