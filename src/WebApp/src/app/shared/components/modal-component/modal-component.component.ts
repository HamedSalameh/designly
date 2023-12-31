import { Component, ElementRef, EventEmitter, Input, Output } from '@angular/core';
import { Strings } from '../../strings';
import { animate, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-modal-component',
  templateUrl: './modal-component.component.html',
  styleUrls: ['./modal-component.component.scss'],
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
export class ModalComponent {

  @Input() title?: string | 'title';
  @Input() content?: string | 'content';

  @Output() closeEvent = new EventEmitter();
  @Output() confirmEvent = new EventEmitter();
  
  Okay: string = Strings.Ok;
  Cancel: string = Strings.Cancel;

  constructor(private element: ElementRef) {
  } 

  cancel() {
    this.element.nativeElement.remove();
    this.closeEvent.emit();
  }

  confirm() {
    this.element.nativeElement.remove();
    this.confirmEvent.emit();
  }
}
