import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class AccountService {
    private serviceAddress = '/accounts-api/v1';

    constructor(private http: HttpClient) { }

    public getAccountUsers(accountId: string): Observable<any> {
        console.debug('[AccountService] [getAccountUsers] ', accountId);
        return this.http.get(`${this.serviceAddress}/accounts/${accountId}/users`);
    }

    public getAccounts(): Observable<any> {
        console.debug('[AccountService] [getAccounts]');
        return this.http.post(`${this.serviceAddress}/accounts/search`, {});        
    }
}