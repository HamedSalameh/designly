import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientsManagementComponent } from './components/clients-management/clients-management.component';

const routes: Routes = [
  { path: '', component: ClientsManagementComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ClientsRoutingModule {}
