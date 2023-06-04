import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Client } from '../../models/client.model';
@Component({
  selector: 'app-client-jacket',
  templateUrl: './client-jacket.component.html',
  styleUrls: ['./client-jacket.component.scss']
})
export class ClientJacketComponent {

  @Input() client: Client | undefined;

  @Output() CloseClientJacket: EventEmitter<any> = new EventEmitter();

  constructor() { }

  onClose() {
    this.client = undefined;
    this.CloseClientJacket.emit();
  }
}
