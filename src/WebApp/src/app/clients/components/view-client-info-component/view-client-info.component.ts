import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Observable, Subject, of, switchMap, take, takeUntil, tap } from 'rxjs';
import { ClientsService } from '../../services/clients.service';
import { ClientSelector, SelectedClientIdSelector } from 'src/app/state/client-state/x-selectors.state';
import { getClient } from 'src/app/state/client-state/x-actions.state';
import { Store, select } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/models/application-state.interface.';

@Component({
  selector: 'app-view-client-info',
  templateUrl: './view-client-info.component.html',
  styleUrls: ['./view-client-info.component.scss'],
})
export class ViewClientInfoComponent implements OnDestroy{
  @Output() CloseClient: EventEmitter<any> = new EventEmitter();
  @Output() EditClient: EventEmitter<any> = new EventEmitter();
  @Output() ShareClient: EventEmitter<any> = new EventEmitter();
  @Output() DeleteClient: EventEmitter<any> = new EventEmitter();

  selectedClient$: Observable<any | null> = of(null);
  private unsubscribe$: Subject<void> = new Subject();

  constructor(
    private xStore: Store<IApplicationState>
  ) {
    this.selectedClient$ =  this.xStore.pipe(select(ClientSelector));
  }

  onClose() {
    this.CloseClient.emit();
  }

  onEdit() {
    this.EditClient.emit();
  }

  onShare() {
    this.ShareClient.emit();
  }

  onDelete() {
    this.DeleteClient.emit();
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
