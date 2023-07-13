import { ApplicationInitStatus, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationGuard } from './authentication/authentication.guard';
import { HomeModule } from './home/home.module';
import { LoginComponent } from './login/components/login.component';
import { RouteConfiguration } from './shared/models/route-configuration.model';
import { RouteFactory } from './shared/providers/route-provider.factory';
import { ApplicationModules } from './shared/application-modules';

const ApplicationRoutes = new Map<string, RouteConfiguration>();

const homeRoute = RouteFactory.createRoute(
  ApplicationModules.home.path,
  ApplicationModules.home.label,
  ApplicationModules.home.route,
  ApplicationModules.home.icon
);
ApplicationRoutes.set(ApplicationModules.home.path, homeRoute);

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  {
    path: ApplicationRoutes.get('home')?.path, // 'home',
    canActivate: [AuthenticationGuard],
    data: ApplicationRoutes.get('home'),
    loadChildren: () => import('./home/home.module').then((m) => m.HomeModule),
  },
  { path: '**', redirectTo: '/login' }, // Optional: Redirect any unknown routes to the login page
];

@NgModule({
  imports: [RouterModule.forRoot(routes), HomeModule],
  exports: [RouterModule],
})
export class AppRoutingModule {}
