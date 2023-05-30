import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
})
export class TableComponent {
  
  onRowSelect($event: any) {
    this.rowSelected.emit($event.data);
  }

  selectedItem: any;

  @Input()
  data: any[] = [];

  @Input()
  cols: any[] = [];

  @Output()
  rowSelected: EventEmitter<any> = new EventEmitter();

  constructor() {}
}
