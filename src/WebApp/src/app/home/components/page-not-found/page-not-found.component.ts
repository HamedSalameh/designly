import { Component } from '@angular/core';
import { Location } from '@angular/common';

@Component({
    selector: 'app-page-not-found',
    templateUrl: './page-not-found.component.html',
    styleUrls: ['./page-not-found.component.scss'],
    standalone: false
})
export class PageNotFoundComponent {

  Title: string = $localize`:@@Pages.404:Umm, we can't find that page!`;
  Subtitle: string = $localize`:@@Pages.404Subtitle:404`;
  Message: string = $localize`:@@Pages.404Message:We're sorry, but the page you're looking for doesn't exist.`;
  GoBack: string = $localize`:@@Pages.GoBack:Go back`;

  constructor(private location: Location) { }

  goBack() {
    this.location.back(); // Navigates to the previous page
  }
}
