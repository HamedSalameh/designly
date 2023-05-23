import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationGuard } from './authentication/authentication.guard';
import { HomeModule } from './home/home.module';
import { LoginComponent } from './login/components/login.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  {
    path: 'home',
    canActivate: [AuthenticationGuard],
    loadChildren: () => import('./home/home.module').then((m) => m.HomeModule)
  },
  { path: '**', redirectTo: '/login' } // Optional: Redirect any unknown routes to the login page
];

@NgModule({
  imports: [RouterModule.forRoot(routes), HomeModule],
  exports: [RouterModule]
})
export class AppRoutingModule { }
