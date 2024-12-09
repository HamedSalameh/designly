import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectWorkspaceComponent } from './project-workspace.component';

describe('ProjectWorkspaceComponent', () => {
  let component: ProjectWorkspaceComponent;
  let fixture: ComponentFixture<ProjectWorkspaceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProjectWorkspaceComponent]
    });
    fixture = TestBed.createComponent(ProjectWorkspaceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
