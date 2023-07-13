import { Icons } from "./icons";

export class ApplicationModules {

    static home = {
        label: $localize`:@@Global.Nav.Home:Home`,
        path: 'home',
        icon: Icons.home,
        route: '/home',
        fullRoute: '/home'
    };

    static dashboard = {
        label: $localize`:@@Global.Nav.Dashboard:Dashboard`,
        path: 'dashboard',
        icon: Icons.dashboard,
        route: '/dashboard',
        fullRoute: '/home/dashboard'
    };

    static clients = {
        label: $localize`:@@Global.Nav.Clients:Clients`,
        path: 'clients',
        icon: Icons.clients,
        route: '/clients',
        fullRoute: '/home/clients'
    };

    static projects = {
        label: $localize`:@@Global.Nav.Projects:Projects`,
        path: 'projects',
        icon: Icons.projects,
        route: '/projects',
        fullRoute: '/home/projects'
    };

    static tasks = {
        label: $localize`:@@Global.Nav.Tasks:Tasks`,
        path: 'tasks',
        icon: Icons.tasks,
        route: '/tasks',
        fullRoute: '/home/tasks'
    };

    static communication = {
        label: $localize`:@@Global.Nav.Communication:Communication`,
        path: 'communication',
        icon: Icons.communication,
        route: '/communication',
        fullRoute: '/home/communication'
    };
}
