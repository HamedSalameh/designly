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
  defaultRowsPerPage: number = 10;

  @Input()
  data: any[] = [];

  @Input()
  cols: any[] = [];

  @Input()
  key: any = "";

  @Input()
  RowsPerPageOptions: any[] = [5, 10, 20, 50];

  @Output()
  rowSelected: EventEmitter<any> = new EventEmitter();

  constructor() {
    if (this.key === "") {
      console.warn("key is empty!");
    }
  }
}
