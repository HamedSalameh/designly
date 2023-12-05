import { trigger, transition, style, animate } from '@angular/animations';
import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { getUser } from 'src/app/authentication/state/auth.selectors';

@Component({
  selector: 'app-profile-menu',
  templateUrl: './profile-menu.component.html',
  styleUrls: ['./profile-menu.component.scss'],
  animations: [
    trigger('inOutAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(40px)' }),
        animate('300ms ease-out', style({ opacity: 1, transform: 'translateY(0)' })),
      ]),
      transition(':leave', [
        style({ opacity: 1, transform: 'translateY(0)' }),
        animate('300ms ease-in', style({ opacity: 0, transform: 'translateY(40px)'} )),
      ]),
    ]),
  ],
})
export class ProfileMenuComponent {
  isMenuOpen: boolean = false;
  loggedUser$ = this.store.select(getUser);
  loggedUser: string = '';

  constructor(private store: Store) {
    this.loggedUser$.subscribe((user) => (this.loggedUser = user));
  }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }

  logout() {
    throw new Error('Method not implemented.');
  }

  
}
