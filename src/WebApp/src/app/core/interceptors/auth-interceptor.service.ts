import { HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAccessToken } from 'src/app/authentication/state/auth.selectors';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private store: Store) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    // Get the auth token from the service.
    let authToken = '';
    this.store.select(getAccessToken).subscribe( (token) => {
      authToken = token;
    });

    // Clone the request and replace the original headers with
    // cloned headers, updated with the authorization.
    const authReq = req.clone({
      headers: req.headers.set('Authorization', authToken)
    });

    // send cloned request with header to the next handler.
    return next.handle(authReq);
  }
}