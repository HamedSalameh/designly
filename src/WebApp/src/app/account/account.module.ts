import { isDevMode, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { ACCOUNT_STATE_NAME } from './state/account.state';
import { AccountStateReducer } from './state/account.reducers';
import { EffectsModule } from '@ngrx/effects';
import { AccountEffects } from './state/account.effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    // State management
    StoreModule.forFeature(ACCOUNT_STATE_NAME, AccountStateReducer),
    EffectsModule.forFeature([AccountEffects]),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
      logOnly: !isDevMode(), // Restrict extension to log-only mode
      autoPause: true, // Pauses recording actions and state changes when the extension window is not open
      trace: false, //  If set to true, will include stack trace for every dispatched action, so you can see it in trace tab jumping directly to that part of code
      traceLimit: 75, // maximum stack trace frames to be stored (in case trace option was provided as true)
    })
  ]
})
export class AccountModule { }
