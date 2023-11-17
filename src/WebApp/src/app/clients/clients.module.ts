import { NgModule, isDevMode } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClientsRoutingModule } from './clients-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ClientsComponent } from './components/clients/clients.component';
import { ClientsManagementComponent } from './components/clients-management/clients-management.component';
import { ViewClientInfoComponent } from './components/view-client-info-component/view-client-info.component';
import { ReactiveFormsModule } from '@angular/forms';
import { EditClientComponent } from './components/edit-client/edit-client.component';
import { StoreModule } from '@ngrx/store';
import { xClientStateReducer } from './client-state/x-reducers.state';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { EffectsModule } from '@ngrx/effects';
import { ClientsEffects } from './client-state/x-effects.state';

@NgModule({
  declarations: [
    ClientsComponent,
    ClientsManagementComponent,
    ViewClientInfoComponent,
    EditClientComponent,
  ],
  imports: [
    ClientsRoutingModule,
    ReactiveFormsModule,
    CommonModule,
    SharedModule,
    StoreModule.forFeature('clients', xClientStateReducer),
    EffectsModule.forFeature([ClientsEffects]),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
      logOnly: !isDevMode(), // Restrict extension to log-only mode
      autoPause: true, // Pauses recording actions and state changes when the extension window is not open
      trace: false, //  If set to true, will include stack trace for every dispatched action, so you can see it in trace tab jumping directly to that part of code
      traceLimit: 75, // maximum stack trace frames to be stored (in case trace option was provided as true)
      connectOutsideZone: true // If set to true, the connection is established outside the Angular zone for better performance
    }),
  ],
})
export class ClientsModule {}
