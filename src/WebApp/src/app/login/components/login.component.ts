import { ParseSourceFile } from '@angular/compiler';
import { Component } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/authentication/authentication-service.service';
import { SigninRequest } from 'src/app/authentication/models/signin-request.model';

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

  constructor(private formBuilder: FormBuilder, 
    private authenticationService: AuthenticationService,
    private router: Router) {
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
      
      this.authenticationService.signIn(signinRequest) .subscribe(response => {
        // upon success naviate to the home page
        console.log(response);
        this.router.navigate(['/home']);
      });
    }
  }
}
