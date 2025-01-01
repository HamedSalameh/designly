import { Component } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { SigninRequest } from 'src/app/authentication/models/signin-request.model';
import { loginStart } from 'src/app/authentication/state/auth.actions';
import { SetLoading } from 'src/app/shared/state/shared/shared.actions';
import { isLoading } from 'src/app/shared/state/shared/shared.selectors';
import { LoginStrings } from '../strings';
import { Strings } from 'src/app/shared/strings';
import { getLoginFailedError } from 'src/app/authentication/state/auth.selectors';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  loginPageWelcomeMessageLine1 = LoginStrings.WelcomeMessageLine1;
  loginPageWelcomeMessageLine2 = LoginStrings.WelcomeMessageLine2;
  usernameLabel = Strings.Username;
  passwordLabel = Strings.Password;
  signInLabel = LoginStrings.SignIn;
  signUpLabel = LoginStrings.SignUp;
  signInWithGoogleLabel = LoginStrings.SigninWithGoogle;
  requestPasswordResetLabel = LoginStrings.ResetPassword;
  localizedSigninWithSocialMediaLabel = LoginStrings.LoginWithSocialMedia;
  localizedEmailAddress = Strings.EmailAddress;

  loginForm = new FormGroup({
    username: new FormControl(''),
    password: new FormControl('')
  });

  isLoading$ = this.store.select(isLoading);
  loginError$ = this.store.select(getLoginFailedError);

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
      this.store.dispatch(SetLoading(true));
    }
  }
}
