import { ParseSourceFile } from '@angular/compiler';
import { Component } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { tap, pipe } from 'rxjs';
import { AuthenticationService } from 'src/app/authentication/authentication-service.service';
import { SigninRequest } from 'src/app/authentication/models/signin-request.model';
import { loginStart } from 'src/app/authentication/state/auth.actions';
import { SetLoading } from 'src/app/shared/state/shared/shared.actions';
import { isLoading } from 'src/app/shared/state/shared/shared.selectors';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  loginPageWelcomeMessageLine1 = $localize`:@@Pages.Login.WelcomeLine1:Hello there! Welcome back — we missed you!`;
  loginPageWelcomeMessageLine2 = $localize`:@@Pages.Login.WelcomeLine2:Let's get you signed in to unleash your creativity.`;
  usernameLabel = $localize`:@@Pages.Login.Username:Username`;
  passwordLabel = $localize`:@@Pages.Login.Password:Password`;
  signInLabel = $localize`:@@Pages.Login.SignIn:Sign In`;
  signUpLabel = $localize`:@@Pages.Login.SignUp:Sign Up`;
  signInWithGoogleLabel = $localize`:@@Pages.Login.SignInWithGoogle:Sign In with Google`;
  requestPasswordResetLabel = $localize`:@@Pages.Login.RequestPasswordReset:Request Password Reset`;
  localizedSigninWithSocialMediaLabel = $localize`:@@Pages.Login.SignInWithSocialMedia:Sign In with Social Media`;

  localizedEmailAddress = $localize`:@@Global.ContactInfo.EmailAddress:EmailAddress`;
  
  loginForm = new FormGroup({
    username: new FormControl(''),
    password: new FormControl('')
  });

  isLoading$ = this.store.select(isLoading);

  constructor(private formBuilder: FormBuilder,
    private store: Store) {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  signIn() {
    if (this.loginForm.valid) {
      const signinRequest = new SigninRequest();
      signinRequest.username = this.loginForm.value.username || '';
      signinRequest.password = this.loginForm.value.password || '';
      
      this.store.dispatch(loginStart({ signInRequest: signinRequest }));
      this.store.dispatch(SetLoading( true));
    }
  }
}
