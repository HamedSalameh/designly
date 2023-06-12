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

  onRowUnselect($event: any) {
    console.log("onRowUnselect");
    this.rowSelected.emit(null);
  }

  selectedItem: any;

  @Input()
  data: any[] = [];

  @Input()
  cols: any[] = [];

  @Input()
  key: any = "";

  @Output()
  rowSelected: EventEmitter<any> = new EventEmitter();

  constructor() {
    if (this.key === "") {
      console.warn("key is empty!");
    }
  }
}
