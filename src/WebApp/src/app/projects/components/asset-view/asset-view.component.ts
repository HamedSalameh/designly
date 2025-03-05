import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { Strings } from 'src/app/shared/strings';
import { Property } from '../../models/property.model';
import { buildRealestatePropertyBuilder } from '../../Builders/realestate-property.builder';
import { getActiveProject } from '../../projects-state/projects.selectors';
import { Store } from '@ngrx/store';
import { IApplicationState } from 'src/app/shared/state/app.state';
import { map, of, Subject, switchMap, takeUntil } from 'rxjs';
import { RealestatePropertyService } from '../../services/realestate-property.service';
import { SearchPropertiesRequest } from '../../models/SearchPropertiesRequest';
import { ModalService } from 'src/app/shared/services/modal-service.service';
import { RealestatePropertyStrings } from '../../real-estate-property-strings';

@Component({
  selector: 'app-asset-view',
  templateUrl: './asset-view.component.html',
  styleUrls: ['./asset-view.component.scss']
})
export class AssetViewComponent implements OnInit, OnDestroy {

  @Output() DeleteRealestateProperty: EventEmitter<any> = new EventEmitter();
  // element ref for modelTemplate
  @ViewChild('modalTemplate')
  modalTemplate!: TemplateRef<any>;

  // localized strings
  Title!: string;
  AreaMeasurementUnit!: string;
  NoAssetsFound = Strings.NoAssetsFound;
  isLoading = false;
  PropoertyType = RealestatePropertyStrings.PropertyType;
  PropertyName = RealestatePropertyStrings.PropertyName;
  PropoeryTotalArea = RealestatePropertyStrings.PropoeryTotalArea;

  public RealestateProperty: Property | null = null;
  private ngUnsubscribe = new Subject<void>();

  constructor(
    private realestatePropertyService: RealestatePropertyService,
    private modalService: ModalService,
    private store: Store<IApplicationState>) {
  }

  ngOnInit(): void {
    this.Title = Strings.AssetDetails;
    this.AreaMeasurementUnit = Strings.AreaMeasurementUnit;

    this.store.select(getActiveProject).pipe(
      takeUntil(this.ngUnsubscribe), // Ensures cleanup
      switchMap((activeProject) => {
        if (activeProject?.PropertyId) {
          this.isLoading = true;
          this.RealestateProperty = null;
          return this.realestatePropertyService.getProperty(activeProject.PropertyId);
        } 
        this.RealestateProperty = null;
        return of(null);
      })
    ).subscribe({
      next: (property) => {
        this.isLoading = false;
        if (property) {
          console.log('Property loaded successfully:', property);
          this.RealestateProperty = property;
        } else {
          console.warn('No properties found');
        }
      },
      error: (err) => {
        this.isLoading = false;
        console.error('Error fetching property:', err);
      }
    });
  }

  onDelete() {
    if (!this.modalTemplate) {
      console.error('Modal template is not available.');
      return;
    }
  
    this.modalService.open(this.modalTemplate, {
      title: RealestatePropertyStrings.DeletePropertyTitle,
      content: RealestatePropertyStrings.DeletePropertyMessage,
    }).subscribe(action => {
      if (action === 'confirm') {
        this.DeleteRealestateProperty.emit();
      }
    });
  }

  onEdit() {
    throw new Error('Method not implemented.');
  }

  onAdd() {
    throw new Error('Method not implemented.');
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
