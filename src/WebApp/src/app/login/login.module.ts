import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthenticationModule } from '../authentication/authentication.module';
import { SharedModule } from "../shared/shared.module";

@NgModule({
    declarations: [
        LoginComponent
    ],
    providers: [],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        AuthenticationModule,
        SharedModule
    ]
})
export class LoginModule { }
