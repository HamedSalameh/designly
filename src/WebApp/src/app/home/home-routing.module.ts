import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from '../home/components/home/home.component';
import { RouteConfiguration } from '../shared/models/route-configuration.model';
import { RouteFactory } from '../shared/providers/route-provider.factory';
import { ApplicationModules } from '../shared/application-modules';

const ModuleRoutes = new Map<string, RouteConfiguration>();

const clientRoute = RouteFactory.createRoute(
  ApplicationModules.clients.path,
  ApplicationModules.clients.label,
  ApplicationModules.clients.route,
  ApplicationModules.clients.icon
);
ModuleRoutes.set(ApplicationModules.clients.path, clientRoute);

const dashboardRoute = RouteFactory.createRoute(
  ApplicationModules.dashboard.path,
  ApplicationModules.dashboard.label,
  ApplicationModules.dashboard.route,
  ApplicationModules.dashboard.icon
);
ModuleRoutes.set(ApplicationModules.dashboard.path, dashboardRoute);

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    children: [
      {
        path: ModuleRoutes.get('dashboard')?.path, // 'dashboard
        data: { breadcrumb: ModuleRoutes.get('dashboard')?.breadcrumb },
        loadChildren: () =>
          import('../dashboard/dashboard.module').then(
            (m) => m.DashboardModule
          ),
      },
      {
        path: ModuleRoutes.get('clients')?.path, // 'clients
        data: { breadcrumb: ModuleRoutes.get('clients')?.breadcrumb },
        loadChildren: () =>
          import('../clients/clients.module').then((m) => m.ClientsModule),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HomeRoutingModule {}
