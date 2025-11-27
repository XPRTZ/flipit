import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageTopics } from './manage-topics';

describe('ManageTopics', () => {
  let component: ManageTopics;
  let fixture: ComponentFixture<ManageTopics>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ManageTopics]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageTopics);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
