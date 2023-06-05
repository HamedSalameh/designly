import { Component, Input } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ClientState } from 'src/app/state/client-state/client-state.state';
import { Client } from '../../models/client.model';
import { ClientsServiceService } from '../../services/clients-service.service';

@Component({
  selector: 'app-client-jacket-generalinfo',
  templateUrl: './client-jacket-generalinfo.component.html',
  styleUrls: ['./client-jacket-generalinfo.component.scss'],
})
export class ClientJacketGeneralinfoComponent {
  @Input() client: Client | undefined;

}
