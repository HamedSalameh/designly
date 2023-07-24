import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpRequest,
  HttpInterceptor,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { catchError, delay,
  EMPTY,
  Observable,
  retryWhen,
  tap,
  throwError,
} from 'rxjs';
import {
  ErrorTypes,
  HttpResponseStatusCodes,
  IApplicationError,
  INetworkError,
  IServerError,
} from 'src/app/shared/types';
import { AddNetworkError } from 'src/app/state/error-state/error-state.actions';
import { GlobalErrorHandlerService } from '../services/global-error-handler.service';

@Injectable({
  providedIn: 'root',
})
export class HttpErrorsInterceptorService implements HttpInterceptor {
  /* This category includes status codes that represent errors related to network interactions. 
  These errors typically occur when the client sends a request to the server, but the server refuses
   the request, requires authentication, or takes too long to respond.
  */
  networkErrors = [
    HttpResponseStatusCodes.FORBIDDEN,
    HttpResponseStatusCodes.REQUEST_TIMEOUT,
    HttpResponseStatusCodes.NETWORK_AUTHENTICATION_REQUIRED,    
  ];

  // network connectivity errors
  networkConnectivityErrors = [HttpResponseStatusCodes.NETWORK_ERROR];

  /* server side application errors 
  these errors are returned by the server, are typically caused by incorrect client requests or internal issues on the server.
  */ 
  serverSideApplicationErrors = [
    HttpResponseStatusCodes.UNAUTHORIZED,
    HttpResponseStatusCodes.INTERNAL_SERVER_ERROR,
    HttpResponseStatusCodes.BAD_GATEWAY,
    HttpResponseStatusCodes.SERVICE_UNAVAILABLE,
    HttpResponseStatusCodes.GATEWAY_TIMEOUT,
    HttpResponseStatusCodes.HTTP_VERSION_NOT_SUPPORTED,
    HttpResponseStatusCodes.NOT_IMPLEMENTED,
    HttpResponseStatusCodes.BAD_REQUEST,
    HttpResponseStatusCodes.NOT_FOUND,
    HttpResponseStatusCodes.METHOD_NOT_ALLOWED,
    HttpResponseStatusCodes.NOT_ACCEPTABLE,
    HttpResponseStatusCodes.CONFLICT,
    HttpResponseStatusCodes.GONE,
    HttpResponseStatusCodes.UNSUPPORTED_MEDIA_TYPE,
    HttpResponseStatusCodes.PROCESSING,
    HttpResponseStatusCodes.PERMANENT_REDIRECT,
  ];

  constructor(private store: Store, private globalErrorHandler: GlobalErrorHandlerService) {}

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
              // In some cases, we should retry the request
              (this.networkErrors.includes(error.status) && count == 0) || count >= numberOfRetries) {
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
            type: ErrorTypes.ApplicationError,
            handled: false,
          };
          return throwError(() => serverOrApplicationError);
        }

        if (this.isNetworkConnectivityError(error)) {
          return EMPTY;
        }

        // Network error cannot be handled by the client
        // Hence patch the application state for error handling
        if (this.isNetworkError(error)) {
          const networkServerError: INetworkError = {
            message: 'Network error',
            originalError: error,
            type: ErrorTypes.NetworkError,
            handled: true,
          };

          return throwError( () => networkServerError);
          //this.store.dispatch(new AddNetworkError(networkServerError));
          return EMPTY;
        }

        console.debug('[HttpErrorsInterceptorService] [intercept] [catchError] Returning unknown error ',error);

        const serverError: IServerError = {
          message: 'Unhandled error',
          originalError: error,
          type: ErrorTypes.UnknownError,
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
