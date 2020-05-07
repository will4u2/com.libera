import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeMakerComponent } from './change-maker.component';

describe('ChangeMakerComponent', () => {
  let component: ChangeMakerComponent;
  let fixture: ComponentFixture<ChangeMakerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChangeMakerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeMakerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
