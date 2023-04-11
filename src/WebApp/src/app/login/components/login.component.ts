import { ParseSourceFile } from '@angular/compiler';
import { Component } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { AuthenticationService } from 'src/app/authentication/authentication-service.service';
import { SigninRequest } from 'src/app/authentication/models/signin-request.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  loginForm = new FormGroup({
    username: new FormControl(''),
    password: new FormControl('')
  });

  constructor(private formBuilder: FormBuilder, 
    private authenticationService: AuthenticationService) {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  signUp() {
    console.log('Signup is not yet implemented');
  }

  signIn() {
    if (this.loginForm.valid) {
      const signinRequest = new SigninRequest();
      signinRequest.username = this.loginForm.value.username || '';
      signinRequest.password = this.loginForm.value.password || '';
      
      this.authenticationService.signIn(signinRequest) .subscribe(response => {
        console.log(response);
      });
    }
  }

  signInWithGoogle() {
    console.log('Sign In with Google');
    this.authenticationService.test()
    .subscribe(response => {
      console.log(response);
    });
  }

  requestPasswordReset() {
    console.log('Request Password Reset');
  }

}
