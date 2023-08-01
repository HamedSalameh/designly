import { trigger, transition, style, animate } from '@angular/animations';
import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { Subject, takeUntil } from 'rxjs';
import {
  SelectClient,
  UnselectClient,
  ViewMode,
} from 'src/app/state/client-state/client-state.actions';
import { ClientState } from 'src/app/state/client-state/client-state.state';

@Component({
  selector: 'app-clients-management',
  templateUrl: './clients-management.component.html',
  styleUrls: ['./clients-management.component.scss'],
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(40px)' }),
        animate(
          '300ms ease-out',
          style({ opacity: 1, transform: 'translateY(0)' })
        ),
      ]),
    ]),
    trigger('fadeInLeft', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateX(-20px)' }),
        animate(
          '300ms ease-out',
          style({ opacity: 1, transform: 'translateX(0)' })
        ),
      ]),
    ]),
  ],
})
export class ClientsManagementComponent {
  clientId: string | null = null;
  isInEditMode: boolean = false;
  private unsubscribe$: Subject<void> = new Subject();

  constructor(private store: Store) {
    this.store
      .select(ClientState.applicationState)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((editMode: any) => {
        this.isInEditMode = editMode.editMode;
      });

    this.store.dispatch(new ViewMode());
  }

  onSelectClient($event: string) {
    const selectedClientId = $event as string;
    this.clientId = selectedClientId;
    this.store.dispatch(new SelectClient(selectedClientId));
  }

  onCloseClientJacket() {
    this.clientId = null;
    this.store.dispatch(new UnselectClient());
  }
}
