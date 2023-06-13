import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from '../home/components/home/home.component';

const localizedDashboardLabel = $localize`:@@Global.Nav.Dashboard:Dashboard`;
const localizedClientsLabel = $localize`:@@Global.Nav.Clients:Clients`;

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    children: [
        {
        path: 'dashboard',
        data: { breadcrumb: localizedDashboardLabel },
        loadChildren: () =>
            import('../dashboard/dashboard.module').then((m) => m.DashboardModule)
        },
      {
        path: 'clients',
        data: { breadcrumb: localizedClientsLabel },
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
