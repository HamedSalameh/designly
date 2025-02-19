import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './components/home/home.component';
import { HomeRoutingModule } from './home-routing.module';
import { SideNavModule } from '../side-nav/side-nav.module';
import { HeaderModule } from '../header/header.module';
import { ClientsModule } from '../clients/clients.module';
import { DashboardModule } from '../dashboard/dashboard.module';
import { SharedModule } from '../shared/shared.module';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';

@NgModule({
  declarations: [
    HomeComponent,
    PageNotFoundComponent,
    
  ],
  imports: [
    HomeRoutingModule,
    CommonModule,
    SharedModule,

    // Layout Modules
    SideNavModule,
    HeaderModule,

    // Feature Modules
    ClientsModule,
    DashboardModule
  ]
})
export class HomeModule { }
