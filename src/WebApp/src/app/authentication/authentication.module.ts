import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { AuthenticationReduce } from './state/auth.reducer';
import { StoreModule } from '@ngrx/store';
import { AUTH_STATE_NAME } from './state/auth.selectors';
import { EffectsModule } from '@ngrx/effects';
import { AuthenticationEffects } from './state/auth.effects';

@NgModule({ declarations: [], imports: [CommonModule,
        StoreModule.forFeature(AUTH_STATE_NAME, AuthenticationReduce),
        EffectsModule.forFeature([AuthenticationEffects])], providers: [
        provideHttpClient(withInterceptorsFromDi())
    ] })
export class AuthenticationModule { }
