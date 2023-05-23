import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from '../home/components/home/home.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    children: [
        {
        path: 'dashboard',
        loadChildren: () =>
            import('../dashboard/dashboard.module').then((m) => m.DashboardModule)
        },
      {
        path: 'clients',
        loadChildren: () =>
          import('../clients/clients.module').then((m) => m.ClientsModule)
      },
    ],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HomeRoutingModule {}
