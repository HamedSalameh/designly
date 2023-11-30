import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map } from 'rxjs';
import { AuthenticationService } from './authentication-service.service';
import { Store } from '@ngrx/store';
import { isAuthenticated } from './state/auth.selectors';
import { Buffer } from 'buffer';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationGuard {

  constructor(private router: Router, 
    private store: Store) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    
      return this.store.select(isAuthenticated)
      .pipe(
        map( (isAuthenticated ) => {
          return isAuthenticated ? true : this.router.parseUrl('/login');
        })
      );
  }
}
