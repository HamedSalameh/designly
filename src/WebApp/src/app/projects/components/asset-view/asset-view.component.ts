import { Component } from '@angular/core';
import { Strings } from 'src/app/shared/strings';

@Component({
  selector: 'app-asset-view',
  templateUrl: './asset-view.component.html',
  styleUrls: ['./asset-view.component.scss']
})
export class AssetViewComponent {

  // localized strings
  Title!: string;
  
  ngOnInit(): void {
    this.Title = Strings.AssetDetails;
  }

}
