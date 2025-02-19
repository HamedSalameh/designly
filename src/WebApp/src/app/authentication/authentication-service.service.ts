import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SigninRequest } from './models/signin-request.model';
import { SigninResponse } from './models/signin-response.model';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  serviceAddress: string = "https://localhost:7119/api/v1/Identity";
  
  constructor(private httpClient: HttpClient) {}

  /**
   * Sign in method that sends user credentials to the backend
   * Backend will set secure HttpOnly cookies with access & refresh tokens
   */
  public signIn(signinRequest: SigninRequest): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('Username', signinRequest.username);
    formData.append('Password', signinRequest.password);

    return this.httpClient.post<SigninResponse>(
      `${this.serviceAddress}/signin`,
      formData,
      {
        headers: new HttpHeaders({ Accept: 'application/json' }),
        withCredentials: true, // Ensures cookies are included in requests
      }
    );
  }

  /**
   * Logout method to clear cookies on the backend
   */
  public signOut(): Observable<any> {
    return this.httpClient.post(
      `${this.serviceAddress}/signout`,
      {},
      { withCredentials: true } // Ensure cookies are included for session invalidation
    );
  }

  /**
   * Silent refresh method to refresh the JWT using the refresh token
   * The backend will handle issuing a new access token and setting it in cookies
   */
  public refreshToken(): Observable<any> {
    return this.httpClient.post(
      `${this.serviceAddress}/refresh-token`,
      {},
      { withCredentials: true } // Backend will refresh the HttpOnly token
    );
  }

  /**
   * Method to revoke the refresh token
   */
  public revokeToken(): Observable<any> {
    return this.httpClient.post(
      `${this.serviceAddress}/revoke-token`,
      {},
      { withCredentials: true } // Backend will revoke the refresh token
    );
  }

  /**
   * Method to check if the user is authenticated
   */
  public isAuthenticated(): Observable<any> {
    return this.httpClient.get(
      `${this.serviceAddress}/is-authenticated`,
      { withCredentials: true } // Backend will check for the HttpOnly token
    );
  }
}
