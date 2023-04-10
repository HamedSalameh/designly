import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SigninRequest } from './models/signin-request.model';
import { SigninResponse } from './models/signin-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  serviceAddress: string = "https://localhost:7246";

  constructor(private httpClient: HttpClient) {
  }

  public signIn(signinRequest: SigninRequest): Observable<any> {

    const formData: FormData = new FormData();
    formData.append('Username', signinRequest.username);
    formData.append('Password', signinRequest.password);

    const httpOptions = {
      headers: new HttpHeaders({
        'Accept': 'application/json'
      })
    };

    return this.httpClient.post<SigninResponse>(`${this.serviceAddress}/signin`, formData, httpOptions);
  }
}
