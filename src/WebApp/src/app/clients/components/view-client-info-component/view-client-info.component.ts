import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Observable, Subject, of, switchMap, take, takeUntil, tap } from 'rxjs';
import { ClientSelector } from 'src/app/clients/client-state/clients.selectors';
import { Store, select } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/state/app.state';

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
