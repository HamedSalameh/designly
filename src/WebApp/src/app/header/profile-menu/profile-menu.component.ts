import { trigger, transition, style, animate } from '@angular/animations';
import { Component, HostListener } from '@angular/core';
import { Store } from '@ngrx/store';
import { logout } from 'src/app/authentication/state/auth.actions';
import { getUser, isAuthenticated } from 'src/app/authentication/state/auth.selectors';
import { HeaderStrings } from '../strings';
import { Strings } from 'src/app/shared/strings';
import { resetClientsState } from 'src/app/clients/client-state/clients.actions';
import { globalResetState, resetSharedState } from 'src/app/shared/state/shared/shared.actions';
import { filter, Subject, takeUntil } from 'rxjs';

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

  localizedEditProfile = HeaderStrings.EditProfile;
  localizedLogout = Strings.Logout;

  isMenuOpen: boolean = false;
  isAuthenticated$ = this.store.select(isAuthenticated);
  loggedUser$ = this.store.select(getUser);
  loggedUser: string = '';

  private unsubscribe$ = new Subject<void>();

  constructor(private store: Store) {

    // Subscribe to user details only after authentication is resolved
    this.loggedUser$
      .pipe(
        filter(user => user !== null), // Ensure the user exists
        takeUntil(this.unsubscribe$)
      )
      .subscribe((user) => {
        this.loggedUser = `${user?.GivenName} ${user?.FamilyName}`;
      });
  }
  

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }

  logout() {
    this.store.dispatch(globalResetState());
    //this.store.dispatch(resetSharedState());
    //this.store.dispatch(resetClientsState());
    this.store.dispatch(logout());
  }

  @HostListener('document:click', ['$event'])
  onClick(event: any) {
    if (!event.target.closest('.profile-menu')) {
      this.isMenuOpen = false;
    }
  }

  ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  
}
