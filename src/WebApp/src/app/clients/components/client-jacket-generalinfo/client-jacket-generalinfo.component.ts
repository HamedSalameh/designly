import { Component, Input } from '@angular/core';
import { Client } from '../../models/client.model';

@Component({
  selector: 'app-client-jacket-generalinfo',
  templateUrl: './client-jacket-generalinfo.component.html',
  styleUrls: ['./client-jacket-generalinfo.component.scss']
})
export class ClientJacketGeneralinfoComponent {

  @Input() client: Client | undefined;

}
