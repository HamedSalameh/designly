import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxsModule } from '@ngxs/store';
import { ChipsModule } from 'primeng/chips';
import { TableModule } from 'primeng/table';
import { TableComponent } from 'src/app/shared/components/table/table.component';
import { ClientState } from 'src/app/state/client-state/client-state.state';
import { ClientsModule } from '../../clients.module';
import { ClientsComponent } from '../clients/clients.component';
import { ClientsManagementComponent } from './clients-management.component';

describe('ClientsManagementComponent', () => {
  let component: ClientsManagementComponent;
  let fixture: ComponentFixture<ClientsManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        ClientsManagementComponent,
        ClientsComponent,
        TableComponent,
      ],
      imports: [
        NgxsModule.forRoot([ClientState]),
        HttpClientTestingModule,
        ChipsModule,
        TableModule,
        BrowserAnimationsModule,
        ReactiveFormsModule
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ClientsManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
