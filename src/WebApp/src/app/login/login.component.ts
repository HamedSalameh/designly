import { Component } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';

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

  constructor(private formBuilder: FormBuilder) {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  signUp() {
    console.log('Signup is not yet implemented');
  }

  signIn() {
    console.log('Signin is not yet implemented');
  }

  signInWithGoogle() {
    console.log('Sign In with Google');
  }

  requestPasswordReset() {
    console.log('Request Password Reset');
  }

}
