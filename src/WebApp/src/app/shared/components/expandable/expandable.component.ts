import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-expandable',
  templateUrl: './expandable.component.html',
  styleUrls: ['./expandable.component.scss']
})
export class ExpandableComponent {
  
  // Customize button text via inputs
  @Input() expandedText: string = 'Collapse';
  @Input() collapsedText: string = 'Expand';
  // Use a string value to set the max-height when expanded (adjust as needed)
  @Input() maxHeight: string = '500px';

  isExpanded = false;

  toggleExpand(): void {
    this.isExpanded = !this.isExpanded;
  }
}