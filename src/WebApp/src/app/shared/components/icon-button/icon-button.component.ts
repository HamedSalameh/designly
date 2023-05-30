import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-icon-button',
  templateUrl: './icon-button.component.html',
  styleUrls: ['./icon-button.component.scss']
})
export class IconButtonComponent {
  @Input() icon: string | undefined;
  @Input() text: string | undefined;
  @Input() disabled: boolean = false;

  @Output() buttonClick: EventEmitter<void> = new EventEmitter<void>();

  handleClick(): void {
    this.buttonClick.emit();
  }
}
