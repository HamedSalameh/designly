import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NgxsModule } from '@ngxs/store';
import { ClientState } from 'src/app/state/client-state/client-state.state';

import { ClientJacketComponent } from './client-jacket.component';

describe('ClientJacketComponent', () => {
  let component: ClientJacketComponent;
  let fixture: ComponentFixture<ClientJacketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClientJacketComponent ],
      imports: [ HttpClientTestingModule, NgxsModule.forRoot([ClientState]) ]
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
