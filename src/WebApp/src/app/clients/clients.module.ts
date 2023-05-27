import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClientsRoutingModule } from './clients-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ClientJacketComponent } from './components/client-jacket/client-jacket.component';
import { ClientsComponent } from './components/clients/clients.component';
import { ClientsManagementComponent } from './components/clients-management/clients-management.component';

@NgModule({
  declarations: [
    ClientsComponent,
    ClientJacketComponent,
    ClientsManagementComponent
    
  ],
  imports: [
    ClientsRoutingModule,
    CommonModule,
    SharedModule
  ]
})
export class ClientsModule { }
