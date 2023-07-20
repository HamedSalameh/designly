import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpRequest,
  HttpInterceptor,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  catchError,
  delay,
  EMPTY,
  Observable,
  retryWhen,
  tap,
  throwError,
} from 'rxjs';
import {
  HttpResponseStatusCodes,
  IApplicationError,
  IServerError,
} from 'src/app/shared/types';

@Injectable({
  providedIn: 'root',
})
export class HttpErrorsInterceptorService implements HttpInterceptor {
  networkErrors = [
    HttpResponseStatusCodes.FORBIDDEN,
    HttpResponseStatusCodes.UNAUTHORIZED,
    HttpResponseStatusCodes.REQUEST_TIMEOUT,
    HttpResponseStatusCodes.INTERNAL_SERVER_ERROR,
    HttpResponseStatusCodes.NOT_IMPLEMENTED,
    HttpResponseStatusCodes.BAD_GATEWAY,
    HttpResponseStatusCodes.SERVICE_UNAVAILABLE,
    HttpResponseStatusCodes.GATEWAY_TIMEOUT,
    HttpResponseStatusCodes.HTTP_VERSION_NOT_SUPPORTED,
    HttpResponseStatusCodes.VARIANT_ALSO_NEGOTIATES,
    HttpResponseStatusCodes.INSUFFICIENT_STORAGE,
    HttpResponseStatusCodes.NETWORK_AUTHENTICATION_REQUIRED,
  ];

  // network connectivity errors
  networkConnectivityErrors = [HttpResponseStatusCodes.NETWORK_ERROR];

  // server side application errors
  // These errors are returned by the server
  serverSideApplicationErrors = [
    HttpResponseStatusCodes.BAD_REQUEST,
    HttpResponseStatusCodes.NOT_FOUND,
    HttpResponseStatusCodes.METHOD_NOT_ALLOWED,
    HttpResponseStatusCodes.NOT_ACCEPTABLE,
    HttpResponseStatusCodes.PROXY_AUTHENTICATION_REQUIRED,
    HttpResponseStatusCodes.CONFLICT,
    HttpResponseStatusCodes.GONE,
    HttpResponseStatusCodes.LENGTH_REQUIRED,
    HttpResponseStatusCodes.PRECONDITION_FAILED,
    HttpResponseStatusCodes.PAYLOAD_TO_LARGE,
    HttpResponseStatusCodes.URI_TOO_LONG,
    HttpResponseStatusCodes.UNSUPPORTED_MEDIA_TYPE,
    HttpResponseStatusCodes.RANGE_NOT_SATISFIABLE,
    HttpResponseStatusCodes.EXPECTATION_FAILED,
    HttpResponseStatusCodes.IM_A_TEAPOT,
    HttpResponseStatusCodes.UPGRADE_REQUIRED,
    HttpResponseStatusCodes.PROCESSING,
    HttpResponseStatusCodes.MULTI_STATUS,
    HttpResponseStatusCodes.IM_USED,
    HttpResponseStatusCodes.PERMANENT_REDIRECT,
    HttpResponseStatusCodes.UNPROCESSABLE_ENTRY,
    HttpResponseStatusCodes.LOCKED,
    HttpResponseStatusCodes.FAILED_DEPENDENCY,
  ];

  constructor() {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    console.debug('[HttpErrorsInterceptorService] [intercept] ', req.url);
    const numberOfRetries = 3;
    let count = 0;

    return next.handle(req).pipe(
      retryWhen((errors) =>
        errors.pipe(
          tap((error) => {
            if (
              (!this.networkErrors.includes(error.status) && count == 0) ||
              count >= numberOfRetries
            ) {
              console.error(error.message);

              throw error;
            }

            count++;
          }),
          delay(count * 1000 + this.randomInteger(1, 100))
        )
      ),
      catchError((error: HttpErrorResponse) => {
        console.debug('[HttpErrorsInterceptorService] [intercept] [catchError] ');

        // Handle errors nased on status code: networkErrors, networkConnectivityErrors, serverSideApplicationErrors
        if (this.isServerSideApplicationError(error)) {
          // This should be forwarded to an application error handler
          const serverOrApplicationError: IApplicationError = {
            message: 'Server side application error',
            originalError: error,
            type: 'common',
          };

          return throwError(() => serverOrApplicationError);
        }

        if (this.isNetworkConnectivityError(error)) {
          return EMPTY;
        }

        if (this.isNetworkError(error)) {
          const networkServerError: IServerError = {
            message: 'Network error',
            originalError: error,
            type: 'common',
            handled: true,
          };

          return throwError(() => networkServerError);
        }

        console.debug('[HttpErrorsInterceptorService] [intercept] [catchError] Returning unknown error ',error);

        const serverError: IServerError = {
          message: 'Unhandled error',
          originalError: error,
          type: 'common',
          handled: false,
        };

        return throwError(() => serverError);
      })
    );
  }

  private randomInteger(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }

  private isServerSideApplicationError(error: HttpErrorResponse): boolean {
    // Check if error status code is one of the server side application errors
    return this.serverSideApplicationErrors.includes(error.status);
  }

  private isNetworkConnectivityError(error: HttpErrorResponse): boolean {
    // Check if error status code is one of the network connectivity errors
    return this.networkConnectivityErrors.includes(error.status);
  }

  private isNetworkError(error: HttpErrorResponse): boolean {
    // Check if error status code is one of the network errors
    return this.networkErrors.includes(error.status);
  }
}
