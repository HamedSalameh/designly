import { trigger, transition, style, animate } from '@angular/animations';
import { Component, HostListener } from '@angular/core';
import { Store } from '@ngrx/store';
import { logout } from 'src/app/authentication/state/auth.actions';
import { getUser } from 'src/app/authentication/state/auth.selectors';
import { HeaderStrings } from '../strings';
import { Strings } from 'src/app/shared/strings';
import { resetClientsState } from 'src/app/clients/client-state/clients.actions';
import { globalResetState, resetSharedState } from 'src/app/shared/state/shared/shared.actions';
import { Subject, takeUntil } from 'rxjs';

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
  loggedUser$ = this.store.select(getUser);
  loggedUser: string = '';

  private unsubscribe$ = new Subject<void>();

  constructor(private store: Store) {
    this.loggedUser$
    .pipe(takeUntil(this.unsubscribe$))
    .subscribe((user) => {
      if (!user) {
        this.loggedUser = '';
        this.store.dispatch(logout());
      }
      this.loggedUser = `${user?.given_name} ${user?.family_name}`;
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
