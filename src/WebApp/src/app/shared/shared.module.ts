import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconButtonComponent } from './components/icon-button/icon-button.component';



@NgModule({
  declarations: [
    IconButtonComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    IconButtonComponent
  ]
})
export class SharedModule { }
