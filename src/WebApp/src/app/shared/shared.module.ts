import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconButtonComponent } from './components/icon-button/icon-button.component';
import { TableComponent } from './components/table/table.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';

import { TableModule } from 'primeng/table';
import { ChipsModule } from 'primeng/chips';
import { MessagesModule } from 'primeng/messages';
import { InlineMessageComponent } from './components/inline-message/inline-message.component';

@NgModule({
  declarations: [
    IconButtonComponent,
    TableComponent,
    BreadcrumbComponent,
    InlineMessageComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,

    TableModule,
    ChipsModule,
    MessagesModule
  ],
  exports: [
    IconButtonComponent,
    TableComponent,
    BreadcrumbComponent,
    InlineMessageComponent,
  ]
})
export class SharedModule { }
