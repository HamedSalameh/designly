import { TestBed } from '@angular/core/testing';

import { HttpErrorsInterceptorService } from './http-errors-interceptor.service';

describe('HttpErrorsInterceptorService', () => {
  let service: HttpErrorsInterceptorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HttpErrorsInterceptorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
