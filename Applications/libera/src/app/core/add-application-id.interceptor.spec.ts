import { TestBed } from '@angular/core/testing';

import { AddApplicationIdInterceptor } from './add-application-id.interceptor';

describe('AddApplicationIdInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      AddApplicationIdInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: AddApplicationIdInterceptor = TestBed.inject(AddApplicationIdInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
