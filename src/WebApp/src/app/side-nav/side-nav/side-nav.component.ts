import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss'],
})
export class SideNavComponent {
  dashboardTitle = $localize`:@@sideNav.Dashboard:Dashboard`;
  projectsTitle = $localize`:@@sideNav.Projects:Projects`;
  tasksTitle = $localize`:@@sideNav.Tasks:Tasks`;
  clientsTitle = $localize`:@@sideNav.Clients:Clients`;
  communicationTitle = $localize`:@@sideNav.Communication:Communication`;
  analyticsTitle = $localize`:@@sideNav.Analytics:Analytics`;

  constructor(private router: Router, private domSanitizer: DomSanitizer) {}

  navigateToDashboard() {
    this.router.navigate(['/home/dashboard']);
  }

  navigateToClient() {
    this.router.navigate(['/home/clients']);
  }
}