import { Component, EventEmitter, Input, Output, TemplateRef, ViewChild } from '@angular/core';
import { Strings } from 'src/app/shared/strings';
import { Property } from '../../models/property.model';
import { buildRealestatePropertyBuilder } from '../../Builders/realestate-property.builder';
import { getActiveProject } from '../../projects-state/projects.selectors';
import { Store } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { map, of, switchMap } from 'rxjs';
import { RealestatePropertyService } from '../../services/realestate-property.service';
import { SearchPropertiesRequest } from '../../models/SearchPropertiesRequest';
import { ModalService } from 'src/app/shared/services/modal-service.service';
import { RealestatePropertyStrings } from '../../real-estate-property-strings';

@Component({
  selector: 'app-asset-view',
  templateUrl: './asset-view.component.html',
  styleUrls: ['./asset-view.component.scss']
})
export class AssetViewComponent {

  @Output() DeleteRealestatePropoerty: EventEmitter<any> = new EventEmitter();
  // element ref for modelTemplate
  @ViewChild('modalTemplate')
  modalTemplate!: TemplateRef<any>;

  // localized strings
  Title!: string;
  AreaMeasurementUnit!: string;
  NoAssetsFound = Strings.NoAssetsFound;
  isLoading = false;

  public RealesateProperties: Property[] = [];

  constructor(
    private realestatePropertyService: RealestatePropertyService,
    private modalService: ModalService,
    private store: Store<IApplicationState>) {
  }

  ngOnInit(): void {
    this.Title = Strings.AssetDetails;
    this.AreaMeasurementUnit = Strings.AreaMeasurementUnit;

    this.store.select(getActiveProject).pipe(
      switchMap((activeProject) => {
        if (activeProject && activeProject.PropertyId) {
          this.isLoading = true;

          return this.realestatePropertyService.getProperty(activeProject.PropertyId);
        }
        console.warn('Active project not found');
        return of(null); // Return a null observable if no active project
      }),
      switchMap((response) => {
        if (response) {
          this.isLoading = false;
          // Process the response into an array of Property objects using the builder function
          return of(response);
          //return of(buildRealestatePropertyBuilder(response));
        }
        return of(); // Return an empty array observable if no response
      })
    ).subscribe({
      next: (property) => {
        if (property) {
          console.log('Property loaded successfully:', property);
          this.RealesateProperties = [property] ;
        } else {
          console.warn('No properties found');
        }
        this.isLoading = false;
      },
      error: (err) => console.error('Error fetching property:', err),
    });
  }

  onDelete() {
    this.modalService.open(this.modalTemplate, {
      title: RealestatePropertyStrings.DeletePropertyTitle,
      content: RealestatePropertyStrings.DeletePropertyMessage,
    }).subscribe(action => {
      console.log(action);
      if (action === 'confirm') {
        this.DeleteRealestatePropoerty.emit();
      }
    });
  }

  onEdit() {
    throw new Error('Method not implemented.');
  }

  onAdd() {
    throw new Error('Method not implemented.');
  }
}
