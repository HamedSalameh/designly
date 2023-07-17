import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApplicationModules } from 'src/app/shared/application-modules';
import { ApplicationModule } from 'src/app/shared/models/application-module.model';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss'],
})
export class SideNavComponent {
  modules: ApplicationModule[] = [];

  constructor(private router: Router) {

    this.modules.push(ApplicationModules.home);
    this.modules.push(ApplicationModules.dashboard);
    this.modules.push(ApplicationModules.clients);
    this.modules.push(ApplicationModules.projects);
    this.modules.push(ApplicationModules.tasks);
    this.modules.push(ApplicationModules.communication);  
  }

  navigateToModule(module: ApplicationModule) {
    if (module.fullRoute) {
      this.router.navigate([module.fullRoute]);
    } else {
      console.error(`Module ${module.label} does not have a route defined.`);
    }
    
  }
}
