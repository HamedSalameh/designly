import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpRequest,
  HttpInterceptor,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
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
  IError,
  INetworkError
} from 'src/app/shared/types';

@Injectable({
  providedIn: 'root',
})
export class HttpErrorsInterceptorService implements HttpInterceptor {
  httpErrors = [
    HttpResponseStatusCodes.FORBIDDEN,
    HttpResponseStatusCodes.REQUEST_TIMEOUT,
    HttpResponseStatusCodes.NETWORK_AUTHENTICATION_REQUIRED,
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
    HttpResponseStatusCodes.NETWORK_ERROR
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
              // In some cases, we should retry the request
              (this.httpErrors.includes(error.status) && count == 0) || count >= numberOfRetries) {
              console.debug('[HttpErrorsInterceptorService] [intercept] [retryWhen] ', error);
              throw error;
            }

            count++;
          }),
          delay(count * 1000 + this.randomInteger(1, 100))
        )
      ),
      catchError((error: HttpErrorResponse) => {
        console.debug('[HttpErrorsInterceptorService] [intercept] [catchError] ');
        if (this.isNetworkError(error)) {
          const networkError: INetworkError = {
            message: 'Network error',
            originalError: error,
            type: ErrorTypes.NetworkError,
            handled: true,
          };

          return throwError( () => networkError);
          return EMPTY;
        }

        const unknownError: IError = {
          message: 'Unhandled error',
          originalError: error,
          type: ErrorTypes.UnknownError,
          handled: false,
        };

        return throwError(() => unknownError);
      })
    );
  }

  private randomInteger(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }

  private isNetworkError(error: HttpErrorResponse): boolean {
    // Check if error status code is one of the network errors
    return this.httpErrors.includes(error.status);
  }
}
