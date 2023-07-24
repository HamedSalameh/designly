import { TestBed } from '@angular/core/testing';

import { ToastMessageServiceService } from './toast-message-service.service';

describe('ToastMessageServiceService', () => {
  let service: ToastMessageServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ToastMessageServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
