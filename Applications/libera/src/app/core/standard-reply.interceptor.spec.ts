import { TestBed } from '@angular/core/testing';

import { StandardReplyInterceptor } from './standard-reply.interceptor';

describe('StandardReplyInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      StandardReplyInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: StandardReplyInterceptor = TestBed.inject(StandardReplyInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
