import { TestBed } from '@angular/core/testing';

import { TillDataService } from './till-data.service';

describe('TillDataService', () => {
  let service: TillDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TillDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
