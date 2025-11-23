import { TestBed } from '@angular/core/testing';

import { Catégorie } from './catégorie';

describe('Catégorie', () => {
  let service: Catégorie;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Catégorie);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
