import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DynamicDialogModule } from 'primeng/dynamicdialog';

import { ClientsRoutingModule } from './clients-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ClientJacketComponent } from './components/client-jacket/client-jacket.component';
import { ClientsComponent } from './components/clients/clients.component';
import { ClientsManagementComponent } from './components/clients-management/clients-management.component';
import { ViewClientInfoComponent } from './components/view-client-info-component/view-client-info.component';
import { ReactiveFormsModule } from '@angular/forms';
import { EditClientComponent } from './components/edit-client/edit-client.component';
import { MessagesModule } from 'primeng/messages';

@NgModule({
  declarations: [
    ClientsComponent,
    ClientJacketComponent,
    ClientsManagementComponent,
    ViewClientInfoComponent,
    EditClientComponent
    
  ],
  imports: [
    ClientsRoutingModule,
    ReactiveFormsModule,
    CommonModule,
    SharedModule,

    DynamicDialogModule,
    MessagesModule
  ]
})
export class ClientsModule { }
