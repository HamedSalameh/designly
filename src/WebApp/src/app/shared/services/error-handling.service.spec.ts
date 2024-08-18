import { TestBed } from '@angular/core/testing';

import { HttpErrorHandlingService } from './error-handling.service';

describe('ErrorHandlingService', () => {
  let service: HttpErrorHandlingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HttpErrorHandlingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
