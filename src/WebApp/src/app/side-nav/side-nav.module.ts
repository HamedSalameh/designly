import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SideNavComponent } from './side-nav/side-nav.component';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    SideNavComponent
  ],
  imports: [
    CommonModule,
    SharedModule
  ],
  exports: [
    SideNavComponent
  ]
})
export class SideNavModule { }
