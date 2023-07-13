import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApplicationModules } from 'src/app/shared/application-modules';

export interface ApplicationModule {
  title: string;
  icon: string;
  routing?: string;
}

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss'],
})
export class SideNavComponent {
  homeTitle = ApplicationModules.home.label; // $localize`:@@Global.Nav.Home:Home`
  dashboardTitle = ApplicationModules.dashboard.label; // $localize`:@@Global.Nav.Dashboard:Dashboard`;
  projectsTitle = $localize`:@@Global.Nav.Projects:Projects`;
  tasksTitle = $localize`:@@Global.Nav.Tasks:Tasks`;
  clientsTitle = ApplicationModules.clients.label; // $localize`:@@Global.Nav.Clients:Clients`
  communicationTitle = $localize`:@@Global.Nav.Communication:Communication`;

  modules: ApplicationModule[] = [];

  constructor(private router: Router) {

    this.modules.push({
      title: ApplicationModules.home.label,
      icon: ApplicationModules.home.icon,
      routing: ApplicationModules.home.fullRoute,
    });

    this.modules.push({
      title: ApplicationModules.dashboard.label,
      icon: ApplicationModules.dashboard.icon,
      routing: ApplicationModules.dashboard.fullRoute,
    });

    this.modules.push({
      title: ApplicationModules.clients.label,
      icon: ApplicationModules.clients.icon,
      routing: ApplicationModules.clients.fullRoute,
    });

    this.modules.push({
      title: ApplicationModules.projects.label,
      icon: ApplicationModules.projects.icon,
      routing: ApplicationModules.projects.fullRoute,
    });

    this.modules.push({
      title: ApplicationModules.tasks.label,
      icon: ApplicationModules.tasks.icon,
      routing: ApplicationModules.tasks.fullRoute,
    });

    this.modules.push({
      title: ApplicationModules.communication.label,
      icon: ApplicationModules.communication.icon,
      routing: ApplicationModules.communication.fullRoute,
    });

  }

  navigateToModule(module: ApplicationModule) {
    this.router.navigate([module.routing]);
  }
}
