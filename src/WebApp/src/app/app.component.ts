import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { checkAuthentication } from './authentication/state/auth.actions';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    standalone: false
})
export class AppComponent {
  title = 'DesignFlow';

  constructor(private store: Store) {
    
  }

  ngOnInit(): void {
    this.store.dispatch(checkAuthentication()); // Only call API once on app load
  }
}
