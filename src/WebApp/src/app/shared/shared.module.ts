import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconButtonComponent } from './components/icon-button/icon-button.component';
import { TableComponent } from './components/table/table.component';
import { TableModule } from 'primeng/table';

@NgModule({
  declarations: [
    IconButtonComponent,
    TableComponent
  ],
  imports: [
    CommonModule,

    TableModule
  ],
  exports: [
    IconButtonComponent,
    TableComponent
  ]
})
export class SharedModule { }
