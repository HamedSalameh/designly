import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableComponent } from './components/table/table.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { NotificationMessageComponent } from './components/notification-message/notification-message.component';

// SyncFusion
import { GridModule, PageService, SearchService, SortService, ToolbarService } from '@syncfusion/ej2-angular-grids';
// Register Syncfusion license
import { registerLicense } from '@syncfusion/ej2-base';
import { ToastModule } from '@syncfusion/ej2-angular-notifications';
import { ERROR_STATE_NAME } from './state/error-state/error.selectors';
import { ErrorStateReducer } from './state/error-state/error.reducer';
import { StoreModule } from '@ngrx/store';
import { SHARED_STATE_NAME } from './state/shared/shared.selectors';
import { SharedStateReducer } from './state/shared/shared.reducers';
import { LoadingSpinnerComponent } from './components/loading-spinner/loading-spinner.component';
import { ModalComponent } from './components/modal-component/modal-component.component';
import { ModalService } from './services/modal-service.service';
import { StatusChipComponent } from './components/status-chip/status-chip.component';
import { RouterModule } from '@angular/router';
import { DropdownComponent } from './components/dropdown/dropdown.component';

registerLicense('Ngo9BigBOggjHTQxAR8/V1NGaF1cWGhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEZjUX5acXBVRmBdU0FzXQ==');

@NgModule({
  declarations: [
    TableComponent,
    BreadcrumbComponent,
    NotificationMessageComponent,
    LoadingSpinnerComponent,
    ModalComponent,
    StatusChipComponent,
    DropdownComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    StoreModule.forFeature(ERROR_STATE_NAME, ErrorStateReducer),
    StoreModule.forFeature(SHARED_STATE_NAME, SharedStateReducer),

    // syncfusion
    GridModule,
    ToastModule
  ],
  exports: [
    TableComponent,
    BreadcrumbComponent,
    NotificationMessageComponent,   
    LoadingSpinnerComponent,
    ModalComponent,
    StatusChipComponent,
    DropdownComponent,
  ],
  providers: [
    // syncfusion
    SearchService,
    ToolbarService,
    SortService,
    PageService,
    ModalService
  ]
})
export class SharedModule {}
