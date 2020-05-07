import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CoinEditorComponent } from './coin-editor.component';

describe('CoinEditorComponent', () => {
  let component: CoinEditorComponent;
  let fixture: ComponentFixture<CoinEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CoinEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CoinEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
