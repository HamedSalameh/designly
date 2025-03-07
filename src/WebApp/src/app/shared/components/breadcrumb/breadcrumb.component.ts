import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { Breadcrumb } from '../../models/breadcrumb.model';
import { BreadcrumbService } from '../../services/breadcrumb.service';

@Component({
    selector: 'app-breadcrumb',
    templateUrl: './breadcrumb.component.html',
    styleUrls: ['./breadcrumb.component.scss'],
    standalone: false
})
export class BreadcrumbComponent {

  breadcrumbs$: Observable<Breadcrumb[]> ; 
 
  constructor(private readonly breadcrumbService: BreadcrumbService) { 
    this.breadcrumbs$ = breadcrumbService.breadcrumbs$;
  } 
  
}
