import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconButtonComponent } from './components/icon-button/icon-button.component';
import { TableComponent } from './components/table/table.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';

import { TableModule } from 'primeng/table';
import { ChipsModule } from 'primeng/chips';
import { MessagesModule } from 'primeng/messages';
import { ToastModule } from 'primeng/toast';
import { NotificationMessageComponent } from './components/notification-message/notification-message.component';
import { MessageService } from 'primeng/api';
import { ToastMessageService } from './services/toast-message-service.service';

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

    TableModule,
    ChipsModule,
    MessagesModule,
    ToastModule,
  ],
  exports: [
    IconButtonComponent,
    TableComponent,
    BreadcrumbComponent,
    NotificationMessageComponent,
    ToastModule
  ],
  providers: [
    MessageService,
    ToastMessageService
  ]
})
export class SharedModule {}
