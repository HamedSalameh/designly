import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, Subject, switchMap, takeUntil, tap } from 'rxjs';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { deleteRealestatePropertyRequest, setActiveProject } from '../../projects-state/projects.actions';
import { getActiveProject, getProjectById } from '../../projects-state/projects.selectors';

@Component({
  selector: 'app-manage-project',
  templateUrl: './manage-project.component.html',
  styleUrls: ['./manage-project.component.scss']
})
export class ManageProjectComponent implements OnInit, OnDestroy {

  projectIdFromRoute = this.route.params.pipe(map((params) => params['id']));

  private ngUnsubscribe = new Subject<void>();

  constructor(private route: ActivatedRoute, private store: Store<IApplicationState>) {
  }

  ngOnInit(): void {
    this.projectIdFromRoute.pipe(
      takeUntil(this.ngUnsubscribe), // Ensure cleanup
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
    console.debug('Deleting property:', $event);
    this.store.select(getActiveProject).pipe(
      takeUntil(this.ngUnsubscribe), // Ensure cleanup
      tap((project) => {
        const propertyId = project?.PropertyId;
        if (propertyId) {
          this.store.dispatch(deleteRealestatePropertyRequest({ propertyId }));
        }
      })
    ).subscribe();
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

}
