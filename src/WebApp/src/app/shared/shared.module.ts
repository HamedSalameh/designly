import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconButtonComponent } from './components/icon-button/icon-button.component';
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
registerLicense('Ngo9BigBOggjHTQxAR8/V1NGaF1cWGhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEZjUX5acXBVRmBdU0FzXQ==');

@NgModule({
  declarations: [
    IconButtonComponent,
    TableComponent,
    BreadcrumbComponent,
    NotificationMessageComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    StoreModule.forFeature(ERROR_STATE_NAME, ErrorStateReducer),

    // syncfusion
    GridModule,
    ToastModule
  ],
  exports: [
    IconButtonComponent,
    TableComponent,
    BreadcrumbComponent,
    NotificationMessageComponent,    
  ],
  providers: [
    // syncfusion
    SearchService,
    ToolbarService,
    SortService,
    PageService
  ]
})
export class SharedModule {}
