import { Component, Input, OnChanges } from '@angular/core';

@Component({
  selector: 'app-status-chip',
  templateUrl: './status-chip.component.html',
  styleUrls: ['./status-chip.component.scss']
})
export class StatusChipComponent implements OnChanges {

  @Input() status: number = 0;

  @Input() colorMap: { [key: number]: string } = {
    0: '#85888e',  // Not Started - Blue
    1: '#9e77ed',  // In Progress - Orange
    2: '#f04438',  // Delayed - Red
    3: '#17b26a',  // Completed - Green
    4: '#f79009',  // On Hold - Purple
    5: '#333741'   // Cancelled - Grey
  };

  @Input() statusTextMap: { [key: number]: string } = {
    0: $localize `:@@notStartedStatus:Not Started`,
    1: $localize `:@@inProgressStatus:In Progress`,
    2: $localize `:@@delayedStatus:Delayed`,
    3: $localize `:@@completedStatus:Completed`,
    4: $localize `:@@onHoldStatus:On Hold`,
    5: $localize `:@@cancelledStatus:Cancelled`
  };

  statusColor: string = 'blue';
  statusText: string = 'Not Started';

  ngOnChanges(): void {
    this.updateStatus();
  }

  private updateStatus(): void {
    this.statusColor = this.colorMap[this.status] || 'grey';
    this.statusText = this.statusTextMap[this.status] || 'Unknown';
  }
}