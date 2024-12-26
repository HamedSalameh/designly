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

  // Get properties
  getProperties(searchPropertiesRequest: SearchPropertiesRequest): Observable<any> {
    console.debug('[RealestatePropertyService] [getProperties]');

    const headers: HttpHeaders = new HttpHeaders().set('Content-Type', 'application/json');
    // create options
    const options = { headers };

    return this.httpClient
      .post(`${this.serviceAddress}/realestate/properties/search`, searchPropertiesRequest, options);
  }
}


