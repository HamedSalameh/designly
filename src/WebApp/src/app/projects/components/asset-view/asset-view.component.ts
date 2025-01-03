import { Component, Input } from '@angular/core';
import { Strings } from 'src/app/shared/strings';
import { Property } from '../../models/property.model';
import { buildRealestatePropertyBuilder } from '../../Builders/realestate-property.builder';
import { getActiveProject } from '../../projects-state/projects.selectors';
import { Store } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { map, of, switchMap } from 'rxjs';
import { RealestatePropertyService } from '../../services/realestate-property.service';
import { SearchPropertiesRequest } from '../../models/SearchPropertiesRequest';

@Component({
  selector: 'app-asset-view',
  templateUrl: './asset-view.component.html',
  styleUrls: ['./asset-view.component.scss']
})
export class AssetViewComponent {

  // localized strings
  Title!: string;
  AreaMeasurementUnit!: string;

  public RealesateProperties: Property[] = [];

  constructor(
    private realestatePropertyService: RealestatePropertyService,
    private store: Store<IApplicationState>) {
  }

  ngOnInit(): void {
    this.Title = Strings.AssetDetails;
    this.AreaMeasurementUnit = Strings.AreaMeasurementUnit;

    this.store.select(getActiveProject).pipe(
      switchMap((activeProject) => {
        if (activeProject) {
          // Build the request and fetch real estate properties
          const searchPropertiesRequest: SearchPropertiesRequest = {
            id: activeProject.PropertyId, // Assuming PropertyId exists in activeProject
          };
    
          return this.realestatePropertyService.getProperties(searchPropertiesRequest);
        }
        console.warn('Active project not found');
        return of(null); // Return a null observable if no active project
      }),
      switchMap((response) => {
        if (response) {
          // Process the response into an array of Property objects using the builder function
          return of(buildRealestatePropertyBuilder(response));
        }
        return of([]); // Return an empty array observable if no response
      })
    ).subscribe({
      next: (properties) => {
        if (properties && properties.length > 0) {
          console.log('Property loaded successfully:', properties);
          this.RealesateProperties = properties;
        } else {
          console.warn('No properties found');
        }
      },
      error: (err) => console.error('Error fetching property:', err),
    });
    

  }
}
