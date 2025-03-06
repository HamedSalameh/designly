import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ViewClientInfoComponent } from './view-client-info.component';
import { NgxsModule } from '@ngxs/store';
import { ClientState } from 'src/app/state/client-state/client-state.state';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('ViewClientInfoComponent', () => {
  let component: ViewClientInfoComponent;
  let fixture: ComponentFixture<ViewClientInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
    declarations: [ViewClientInfoComponent],
    imports: [NgxsModule.forRoot([ClientState])],
    providers: [provideHttpClient(withInterceptorsFromDi()), provideHttpClientTesting()]
})
    .compileComponents();

    fixture = TestBed.createComponent(ViewClientInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should emit CloseClient event', () => {
    const emitSpy = spyOn(component.CloseClient, 'emit');
    component.onClose();
    expect(emitSpy).toHaveBeenCalled();
  });

  it('should emit EditClient event', () => {
    const emitSpy = spyOn(component.EditClient, 'emit');
    component.onEdit();
    expect(emitSpy).toHaveBeenCalled();
  });

});
