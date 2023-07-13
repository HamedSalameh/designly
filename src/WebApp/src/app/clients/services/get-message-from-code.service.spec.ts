import { TestBed } from '@angular/core/testing';

import { GetMessageFromCodeService } from './get-message-from-code.service';

describe('GetMessageFromCodeService', () => {
  let service: GetMessageFromCodeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GetMessageFromCodeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
