import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TillManagerComponent } from './till-manager.component';

describe('TillManagerComponent', () => {
  let component: TillManagerComponent;
  let fixture: ComponentFixture<TillManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TillManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TillManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
