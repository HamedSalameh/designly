import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent {

  constructor(private router: Router, private domSanitizer: DomSanitizer) {
  }

  navigateToDashboard() {
    this.router.navigate(['/home/dashboard']);
  }

  navigateToClient() {
    this.router.navigate(['/home/clients']);
  }

}
