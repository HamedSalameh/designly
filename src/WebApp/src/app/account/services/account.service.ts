import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SearchAccountUsersRequest } from '../models/search-account-users.request';

@Injectable({
    providedIn: 'root'
})
export class AccountService {
    private serviceAddress = '/accounts-api/v1';

    constructor(private http: HttpClient) { }

    public getAccountUsers(accountId:  string, searchAccountUsersRequest: SearchAccountUsersRequest): Observable<any> {
        console.debug('[AccountService] [getAccountUsers] ', searchAccountUsersRequest);
        return this.http.post(`${this.serviceAddress}/accounts/${accountId}/users/search`, searchAccountUsersRequest);
    }
}