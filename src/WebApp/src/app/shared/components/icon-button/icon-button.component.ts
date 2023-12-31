import { Component, EventEmitter, Input, Output } from '@angular/core';
import { IApplicationState } from '../../state/app.state';
import { Store } from '@ngrx/store';
import { activeModule } from '../../state/shared/shared.selectors';

@Component({
  selector: 'app-icon-button',
  templateUrl: './icon-button.component.html',
  styleUrls: ['./icon-button.component.scss']
})
export class IconButtonComponent {
  @Input() icon: string | undefined;
  @Input() text: string | undefined;
  @Input() disabled: boolean = false;
  @Input() path: string | undefined;

  @Output() buttonClick: EventEmitter<void> = new EventEmitter<void>();

  isActiveModule: boolean = false;

  constructor(private store: Store<IApplicationState>) { 

  }

  ngOnInit(): void {
    this.store.select(activeModule).subscribe(activeModule => {
      this.isActiveModule = this.path == activeModule;
    });
  }
  

  handleClick(): void {
    this.buttonClick.emit();
  }
}
