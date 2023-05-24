import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardViewComponent } from './components/dashboard-view/dashboard-view.component';
import { DashboardRoutingModule } from './dashboard-routing.module';



@NgModule({
  declarations: [
    DashboardViewComponent
  ],
  imports: [
    DashboardRoutingModule,
    CommonModule
  ]
})
export class DashboardModule { }
