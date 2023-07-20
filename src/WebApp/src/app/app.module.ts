import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { CoreModule } from './core/core.module';
import { LoginModule } from './login/login.module';
import { AuthInterceptorService } from './authentication/auth-interceptor.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { HomeModule } from './home/home.module';
import { NgxsModule } from '@ngxs/store';
import { ClientState } from './state/client-state/client-state.state';
import { NgxsLoggerPlugin, NgxsLoggerPluginModule } from '@ngxs/logger-plugin';
import { HttpErrorsInterceptorService } from './core/interceptors/http-errors-interceptor.service';
import { ErrorState } from './state/error-state/error-state.state';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    NgxsModule.forRoot([
      ClientState,
      ErrorState
    ]),
    NgxsLoggerPluginModule.forRoot(),
    CoreModule,

    HomeModule,
    LoginModule
  ],
  providers: [  
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorService, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorsInterceptorService, multi: true },
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
