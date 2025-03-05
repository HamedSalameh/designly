import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { AuthenticationReduce } from './state/auth.reducer';
import { StoreModule } from '@ngrx/store';
import { AUTH_STATE_NAME } from './state/auth.selectors';
import { EffectsModule } from '@ngrx/effects';
import { AuthenticationEffects } from './state/auth.effects';

@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    StoreModule.forFeature(AUTH_STATE_NAME, AuthenticationReduce),
    EffectsModule.forFeature([AuthenticationEffects]),
    
  ],
  providers: [    
  ]
})
export class AuthenticationModule { }
