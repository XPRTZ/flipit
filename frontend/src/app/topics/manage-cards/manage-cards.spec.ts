import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageCards } from './manage-cards';

describe('ManageCards', () => {
  let component: ManageCards;
  let fixture: ComponentFixture<ManageCards>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ManageCards]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageCards);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
