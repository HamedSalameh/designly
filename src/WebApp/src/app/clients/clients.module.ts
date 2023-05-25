import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientsComponent } from './components/clients/clients.component';
import { ClientsRoutingModule } from './clients-routing.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    ClientsComponent
  ],
  imports: [
    ClientsRoutingModule,
    CommonModule,
    SharedModule
  ]
})
export class ClientsModule { }
