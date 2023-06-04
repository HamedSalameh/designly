import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClientsRoutingModule } from './clients-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ClientJacketComponent } from './components/client-jacket/client-jacket.component';
import { ClientsComponent } from './components/clients/clients.component';
import { ClientsManagementComponent } from './components/clients-management/clients-management.component';
import { ClientJacketGeneralinfoComponent } from './components/client-jacket-generalinfo/client-jacket-generalinfo.component';

@NgModule({
  declarations: [
    ClientsComponent,
    ClientJacketComponent,
    ClientsManagementComponent,
    ClientJacketGeneralinfoComponent
    
  ],
  imports: [
    ClientsRoutingModule,
    CommonModule,
    SharedModule
  ]
})
export class ClientsModule { }
