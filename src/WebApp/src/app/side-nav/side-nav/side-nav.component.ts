import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { SetActionsActive } from '@ngrx/store-devtools/src/actions';
import { ApplicationModules } from 'src/app/shared/application-modules';
import { ApplicationModule } from 'src/app/shared/models/application-module.model';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { SetActiveModule } from 'src/app/shared/state/shared/shared.actions';
import { activeModule } from 'src/app/shared/state/shared/shared.selectors';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss'],
})
export class SideNavComponent {
  modules: ApplicationModule[] = [];
  activeModule: string = '';

  constructor(private router: Router, private store: Store<IApplicationState>) {
    this.modules.push(ApplicationModules.home);
    this.modules.push(ApplicationModules.dashboard);
    this.modules.push(ApplicationModules.clients);
    this.modules.push(ApplicationModules.projects);
    this.modules.push(ApplicationModules.tasks);
    this.modules.push(ApplicationModules.communication);

    
  }

  ngOnInit(): void {
    this.store.select(activeModule).subscribe((activeModule) => {
      this.activeModule = activeModule;
    });
  }

  handleClick(module: ApplicationModule): void {
    if (module.fullRoute) {
      this.router.navigate([module.fullRoute]);
      this.store.dispatch(SetActiveModule(module.path));
    } else {
      console.error(`Module ${module.label} does not have a route defined.`);
    }
  }

  isActiveModule(module: string): boolean {
    return this.activeModule == module;
  }
}
