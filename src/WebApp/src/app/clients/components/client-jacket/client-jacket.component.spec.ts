import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientJacketComponent } from './client-jacket.component';

describe('ClientJacketComponent', () => {
  let component: ClientJacketComponent;
  let fixture: ComponentFixture<ClientJacketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClientJacketComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClientJacketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
