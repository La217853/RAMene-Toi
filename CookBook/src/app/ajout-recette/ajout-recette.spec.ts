import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AjoutRecette } from './ajout-recette';

describe('AjoutRecette', () => {
  let component: AjoutRecette;
  let fixture: ComponentFixture<AjoutRecette>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AjoutRecette]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AjoutRecette);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
