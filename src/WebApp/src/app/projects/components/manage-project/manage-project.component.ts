import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, switchMap, tap } from 'rxjs';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { deleteRealestatePropertyRequest, setActiveProject } from '../../projects-state/projects.actions';
import { getActiveProject, getProjectById } from '../../projects-state/projects.selectors';

@Component({
  selector: 'app-manage-project',
  templateUrl: './manage-project.component.html',
  styleUrls: ['./manage-project.component.scss']
})
export class ManageProjectComponent implements OnInit {


  projectIdFromRoute = this.route.params.pipe(map((params) => params['id']));

  constructor(private route: ActivatedRoute, private store: Store<IApplicationState>) {
  }

  ngOnInit(): void {
    this.projectIdFromRoute.pipe(
      switchMap((projectId: string) =>
        this.store.select(getProjectById(projectId)).pipe(
          tap((project) => {
            if (!project) {
              console.warn('Project not found');
            } else {
              // Dispatch the action to set the active project in the store
              this.store.dispatch(setActiveProject({ project }));
            }
          })
        )
      )
    ).subscribe({
      error: (err) => console.error('Error:', err),
    });
  }

  onDeleteRealestatePropoerty($event: any) {
    // get the active project from the store
    console.debug('Deleting property:', $event);
    this.store.select(getActiveProject).pipe(
      tap((project) => {
        const propertyId = project?.PropertyId;
        if (propertyId) {
          this.store.dispatch(deleteRealestatePropertyRequest({ propertyId }));
        }
      })
    ).subscribe();
  }

}
