import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SearchProjectsRequest } from '../models/search-pojects.request';
import { SearchPropertiesRequest } from '../models/SearchPropertiesRequest';

@Injectable({
  providedIn: 'root'
})
export class RealestatePropertyService {

  private serviceAddress = '/projects-api/v1';

  constructor(private httpClient: HttpClient) { }

  getProperty(propertyId: string): Observable<any> {
    console.debug('[RealestatePropertyService] [getProperty]');

    const headers: HttpHeaders = new HttpHeaders().set('Content-Type', 'application/json');
    // create options
    const options = { headers };

    return this.httpClient
      .get(`${this.serviceAddress}/realestate/properties/${propertyId}`, options);
  }

  // Get properties
  getProperties(searchPropertiesRequest: SearchPropertiesRequest): Observable<any> {
    console.debug('[RealestatePropertyService] [getProperties]');

    const headers: HttpHeaders = new HttpHeaders().set('Content-Type', 'application/json');
    // create options
    const options = { headers };

    return this.httpClient
      .post(`${this.serviceAddress}/realestate/properties/search`, searchPropertiesRequest, options);
  }

  // delete property
  deleteProperty(propertyId: string): Observable<any> {
    console.debug('[RealestatePropertyService] [deleteProperty]');

    const headers: HttpHeaders = new HttpHeaders().set('Content-Type', 'application/json');
    // create options
    const options = { headers };

    return this.httpClient
      .delete(`${this.serviceAddress}/realestate/properties/${propertyId}`, options);
  }
}


