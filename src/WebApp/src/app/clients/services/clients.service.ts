import { HttpClient } from '@angular/common/http';
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
    console.log('[ClientsServiceService] [getClient] ', clientId);
    return this.httpClient
      .get<Client>(`${this.serviceAddress}/clients/${clientId}`)
  }

  public addClient(client: Client): Observable<Client> {
    console.log('[ClientsServiceService] [addClient] ', client);
    return this.httpClient
      .post<Client>(`${this.serviceAddress}/clients`, client)
  }

  public deleteClient(clientId: string): Observable<Client> {
    console.log('[ClientsServiceService] [deleteClient] ', clientId);
    return this.httpClient
      .delete<Client>(`${this.serviceAddress}/clients/${clientId}`)
  }

  // An API call to check if a client can be deleted
  public canDeleteClient(clientId: string): Observable<boolean> {
    console.log('[ClientsServiceService] [canDeleteClient] ', clientId);
    return this.httpClient
      .get<boolean>(`${this.serviceAddress}/clients/${clientId}/canDelete`)
  }

  public updateClient(client: Client): Observable<Client> {
    console.log('[ClientsServiceService] [updateClient] ', client);
    return this.httpClient
      .put<Client>(`${this.serviceAddress}/clients/${client.Id}`, client);
  }
}
