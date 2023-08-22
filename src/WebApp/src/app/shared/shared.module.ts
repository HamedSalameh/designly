import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconButtonComponent } from './components/icon-button/icon-button.component';
import { TableComponent } from './components/table/table.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { NotificationMessageComponent } from './components/notification-message/notification-message.component';
import { ToastMessageService } from './services/toast-message-service.service';

import { ChipsModule } from 'primeng/chips';

// SyncFusion
import { GridModule, PageService, SearchService, SortService, ToolbarService } from '@syncfusion/ej2-angular-grids';
// Register Syncfusion license
import { registerLicense } from '@syncfusion/ej2-base';
import { ToastModule } from '@syncfusion/ej2-angular-notifications';
import { ToastComponent } from './components/toast/toast.component';
registerLicense('Ngo9BigBOggjHTQxAR8/V1NGaF1cWGhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEZjUX5acXBVRmBdU0FzXQ==');

@NgModule({
  declarations: [
    IconButtonComponent,
    TableComponent,
    BreadcrumbComponent,
    NotificationMessageComponent,
    ToastComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,

    // syncfusion
    GridModule,
    ToastModule
  ],
  exports: [
    IconButtonComponent,
    TableComponent,
    BreadcrumbComponent,
    NotificationMessageComponent,
    ToastComponent,
    
  ],
  providers: [
    ToastMessageService,

    // syncfusion
    SearchService,
    ToolbarService,
    SortService,
    PageService
  ]
})
export class SharedModule {}
