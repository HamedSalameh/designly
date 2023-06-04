import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientJacketGeneralinfoComponent } from './client-jacket-generalinfo.component';

describe('ClientJacketGeneralinfoComponent', () => {
  let component: ClientJacketGeneralinfoComponent;
  let fixture: ComponentFixture<ClientJacketGeneralinfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClientJacketGeneralinfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClientJacketGeneralinfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
