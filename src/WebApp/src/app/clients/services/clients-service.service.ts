import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Client } from '../models/client.model';

@Injectable({
  providedIn: 'root'
})
export class ClientsServiceService {

  private serviceAddress = 'http://localhost:3000';

  constructor(private httpClient: HttpClient) { }

  public getClients(): Observable<Client[]> {
    return this.httpClient.get<Client[]>(`${this.serviceAddress}/clients`).pipe(
      catchError(this.handleError)
    );
  }

  public getClient(clientId: string): Observable<Client> {
    console.log('[ClientsServiceService] [getClient] ', clientId);
    return this.httpClient.get<Client>(`${this.serviceAddress}/clients/${clientId}`).pipe(
      catchError(this.handleError)
    );
  }

  public addClient(client: Client): Observable<Client> {
    console.log('[ClientsServiceService] [addClient] ', client);
    return this.httpClient.post<Client>(`${this.serviceAddress}/clients`, client).pipe(
      catchError(this.handleError)
    );
  }

  public deleteClient(clientId: string): Observable<Client> {
    console.log('[ClientsServiceService] [deleteClient] ', clientId);
    return this.httpClient.delete<Client>(`${this.serviceAddress}/clients/${clientId}`).pipe(
      catchError(this.handleError)
    );
  }
  
  // An API call to check if a client can be deleted
  public canDeleteClient(clientId: string): Observable<boolean> {
    console.log('[ClientsServiceService] [canDeleteClient] ', clientId);
    return this.httpClient.get<boolean>(`${this.serviceAddress}/clients/${clientId}/canDelete`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    // Customize error handling based on your requirements
    let errorMessage = 'An error occurred';
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.error(errorMessage);
    return throwError(errorMessage);
  }
}
