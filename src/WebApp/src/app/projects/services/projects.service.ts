import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Search } from '@syncfusion/ej2-angular-grids';
import { SearchProjectsRequest } from '../models/search-pojects.request';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {

  private serviceAddress = '/projects-api/v1';

  constructor(private httpClient: HttpClient) {}

  // get projetcs
  getProjects(searchProjectRequest: SearchProjectsRequest): Observable<any> {
    console.debug('[ProjectsService] [getProjects]');

    const headers: HttpHeaders = new HttpHeaders().set('Content-Type', 'application/json');
    // create options
    const options = { headers };

    return this.httpClient
      .post(`${this.serviceAddress}/projects/search`, searchProjectRequest, options);
  }
}
