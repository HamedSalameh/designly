import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { SharedModule } from '../shared/shared.module';
import { ProfileMenuComponent } from './profile-menu/profile-menu.component';



@NgModule({
  declarations: [
    HeaderComponent,
    ProfileMenuComponent
  ],
  imports: [
    CommonModule,
    SharedModule
  ],
  exports: [
    HeaderComponent
  ]
})
export class HeaderModule { }
