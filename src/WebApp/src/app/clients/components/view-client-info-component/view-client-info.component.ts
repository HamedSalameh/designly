import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { Observable, Subject, of, switchMap, take, takeUntil, tap } from 'rxjs';
import { getSingleClient } from 'src/app/clients/client-state/clients.selectors';
import { Store } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { ModalService } from 'src/app/shared/services/modal-service.service';
import { ClientStrings } from '../../strings';

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

  // element ref for modelTemplate
  @ViewChild('modalTemplate')
  modalTemplate!: TemplateRef<any>;

  constructor(
    private xStore: Store<IApplicationState>,
    private modalService: ModalService
  ) {
    this.selectedClient$ = this.xStore.select(getSingleClient);
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
    this.modalService.open(this.modalTemplate, {
      title: ClientStrings.DeleteClientTitle,
      content: ClientStrings.DeleteClientMessage,
      }).subscribe( action => {
        console.log(action);
        if (action === 'confirm') {
          this.DeleteClient.emit();
        }
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
