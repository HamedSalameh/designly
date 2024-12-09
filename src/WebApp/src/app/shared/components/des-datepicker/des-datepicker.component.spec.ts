import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesDatepickerComponent } from './des-datepicker.component';

describe('DesDatepickerComponent', () => {
  let component: DesDatepickerComponent;
  let fixture: ComponentFixture<DesDatepickerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DesDatepickerComponent]
    });
    fixture = TestBed.createComponent(DesDatepickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
