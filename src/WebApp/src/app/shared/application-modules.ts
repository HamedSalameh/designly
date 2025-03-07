import { AppModule } from '../app.module';
import { Icons } from './icons';
import { ApplicationModule } from './models/application-module.model';

export class ApplicationModules {
  static home: ApplicationModule = {
    label: $localize`:@@Global.Nav.Home:Home`,
    path: 'home',
    icon: Icons.home,
    route: 'home'
  };

  static dashboard: ApplicationModule = {
    label: $localize`:@@Global.Nav.Dashboard:Dashboard`,
    path: 'dashboard',
    icon: Icons.dashboard,
    route: 'dashboard'
  };

  static clients: ApplicationModule = {
    label: $localize`:@@Global.Nav.Clients:Clients`,
    path: 'clients',
    icon: Icons.clients,
    route: 'clients'
  };

  static projects: ApplicationModule = {
    label: $localize`:@@Global.Nav.Projects:Projects`,
    path: 'projects',
    icon: Icons.projects,
    route: 'projects'
  };

  static tasks: ApplicationModule = {
    label: $localize`:@@Global.Nav.Tasks:Tasks`,
    path: 'tasks',
    icon: Icons.tasks,
    route: 'tasks'
  };

  static communication: ApplicationModule = {
    label: $localize`:@@Global.Nav.Communication:Communication`,
    path: 'communication',
    icon: Icons.communication,
    route: 'communication'
  };
}
