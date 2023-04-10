import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { Observable } from 'rxjs';
import { share, shareReplay, tap } from 'rxjs/operators';
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

    return this.httpClient.post<SigninResponse>(`${this.serviceAddress}/signin`, formData, httpOptions).pipe(
      tap(res => this.setSession(res)),
      shareReplay()
    );
      
  }

  public test(): Observable<any> {
    let access = localStorage.getItem('access_token');
    const httpOptions = {
      headers: new HttpHeaders({
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + access
      })
    };

    return this.httpClient.get<SigninResponse>(`${this.serviceAddress}/test`, httpOptions);
  }

  private setSession(authResult: SigninResponse) {
    const expiresAt = moment().add(authResult.idToken,'second');

        localStorage.setItem('access_token', authResult.accessToken);
        localStorage.setItem("expires_at", JSON.stringify(expiresAt.valueOf()) );
  }
}
