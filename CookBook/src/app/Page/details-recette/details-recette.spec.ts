import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecetteDetailsComponent } from './details-recette';

describe('DetailsRecette', () => {
  let component: RecetteDetailsComponent;
  let fixture: ComponentFixture<RecetteDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecetteDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecetteDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
