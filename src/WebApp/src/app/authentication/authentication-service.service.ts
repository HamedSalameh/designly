import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { Observable, of, Subject } from 'rxjs';
import { share, shareReplay, tap } from 'rxjs/operators';
import { SigninRequest } from './models/signin-request.model';
import { SigninResponse } from './models/signin-response.model';

export const access_token_key = 'access_token';
export const expires_at_key = 'expires_at';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  serviceAddress: string = "https://localhost:7119/api/v1/Identity";
  //serviceAddress: string = '/auth/v1/Identity';
  constructor(private httpClient: HttpClient) {}

  public signIn(signinRequest: SigninRequest): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('Username', signinRequest.username);
    formData.append('Password', signinRequest.password);

    const httpOptions = {
      headers: new HttpHeaders({
        Accept: 'application/json',
      }),
    };

    return this.httpClient
      .post<SigninResponse>(
        `${this.serviceAddress}/signin`,
        formData,
        httpOptions);
  }
}
