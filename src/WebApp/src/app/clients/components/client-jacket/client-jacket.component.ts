import { Component, Input } from '@angular/core';
import { Client } from '../../models/client.model';
@Component({
  selector: 'app-client-jacket',
  templateUrl: './client-jacket.component.html',
  styleUrls: ['./client-jacket.component.scss']
})
export class ClientJacketComponent {

  @Input() client: Client | undefined;

}
