import { HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private store: Store) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    // do nothing
    // TODO: Check if user had permission
    return next.handle(req);
  }
}