import { TestBed } from '@angular/core/testing';

import { ErrorTranslationService } from './error-translation.service';

describe('ErrorTranslationService', () => {
  let service: ErrorTranslationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ErrorTranslationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
