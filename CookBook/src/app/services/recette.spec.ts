import { TestBed } from '@angular/core/testing';

import { Recette } from './recette';

describe('Recette', () => {
  let service: Recette;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Recette);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
