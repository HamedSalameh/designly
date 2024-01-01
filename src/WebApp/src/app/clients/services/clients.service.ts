import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Client } from '../models/client.model';

@Injectable({
  providedIn: 'root',
})
export class ClientsService {
  private serviceAddress = '/api/v1';

  constructor(private httpClient: HttpClient) {}

  public getClients(): Observable<Client[]> {
    return this.httpClient
      .post<Client[]>(`${this.serviceAddress}/clients/search`, {})
  }

  public getClient(clientId: string): Observable<Client> {
    console.debug('[ClientsServiceService] [getClient] ', clientId);
    return this.httpClient
      .get<Client>(`${this.serviceAddress}/clients/${clientId}`)
  }

  public addClient(client: Client): Observable<string> {
    console.debug('[ClientsServiceService] [addClient] ', client);
    const headers: HttpHeaders = new HttpHeaders().set('Content-Type', 'application/json');
    const responseType: 'text' = 'text';

    // Create options
    const options = { headers, responseType };

    return this.httpClient
      .post(`${this.serviceAddress}/clients`, client, options);
  }

  public deleteClient(clientId: string): Observable<boolean> {
    console.debug('[ClientsServiceService] [deleteClient] ', clientId);
    return this.httpClient
      .delete<boolean>(`${this.serviceAddress}/clients/${clientId}`)
  }

  // An API call to check if a client can be deleted
  public canDeleteClient(clientId: string): Observable<boolean> {
    console.debug('[ClientsServiceService] [canDeleteClient] ', clientId);
    return this.httpClient
      .get<boolean>(`${this.serviceAddress}/clients/${clientId}/canDelete`)
  }

  public updateClient(client: Client): Observable<string> {
    // Define headers and response type
    const headers: HttpHeaders = new HttpHeaders().set('Content-Type', 'application/json');
    const responseType: 'text' = 'text';

    // Create options
    const options = { headers, responseType };

    // Make the call
    const response = this.httpClient
    .put(`${this.serviceAddress}/clients/${client.Id}`, client, options)

    return response;
  }
}
