import { Component, ElementRef, EventEmitter, Input, Output } from '@angular/core';
import { Strings } from '../../strings';

@Component({
  selector: 'app-modal-component',
  templateUrl: './modal-component.component.html',
  styleUrls: ['./modal-component.component.scss']
})
export class ModalComponent {

  @Input() title?: string | 'title';
  @Input() content?: string | 'content';

  @Output() closeEvent = new EventEmitter();
  @Output() confirmEvent = new EventEmitter();
  
  Okay: string = Strings.Ok;
  Cancel: string = Strings.Cancel;

  constructor(private element: ElementRef) {} 

  cancel() {
    this.element.nativeElement.remove();
    this.closeEvent.emit();
  }

  confirm() {
    this.element.nativeElement.remove();
    this.confirmEvent.emit();
  }
}
