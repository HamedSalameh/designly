import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationGuard } from './authentication/authentication.guard';
import { HomeModule } from './home/home.module';
import { LoginComponent } from './login/components/login.component';
import { RouteConfiguration } from './shared/models/route-configuration.model';
import { RouteFactory } from './shared/providers/route-provider.factory';
import { ApplicationModules } from './shared/application-modules';
import { PageNotFoundComponent } from './home/components/page-not-found/page-not-found.component';

const ApplicationRoutes = new Map<string, RouteConfiguration>();
const homePath = ApplicationRoutes.get('home')?.path ?? 'home'; // Default to 'home'

const homeRoute = RouteFactory.createRoute(
  ApplicationModules.home.path,
  ApplicationModules.home.label,
  ApplicationModules.home.route,
  ApplicationModules.home.icon
);
ApplicationRoutes.set(ApplicationModules.home.path, homeRoute);

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: homePath, pathMatch: 'full' },
  {
    path: homePath, // 'home',
    canActivate: [AuthenticationGuard],
    data: ApplicationRoutes.get('home'),
    loadChildren: () => import('./home/home.module').then((m) => m.HomeModule),
  },
  // 404
  {
    path: '404', component: PageNotFoundComponent
  },
  {
    path: '**', redirectTo: '404'
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      //bindToComponentInputs: true,
    }),
    HomeModule],
  exports: [RouterModule]
})
export class AppRoutingModule { }
