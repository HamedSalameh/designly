import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
  ViewEncapsulation 
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { SearchSettingsModel, SelectionSettings, SelectionSettingsModel, ToolbarItems } from '@syncfusion/ej2-angular-grids';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
  encapsulation: ViewEncapsulation.None // This is needed to override the default encapsulation of Angular
})
export class TableComponent implements OnInit {
  // Syncfusion Grid Search Settings
  public searchOptions?: SearchSettingsModel;
  public toolbarOptions?: ToolbarItems[];
  public selectionOptions: SelectionSettingsModel = { type: 'Single', mode: 'Row' };
  
  onRowSelect($event: any) {
    this.rowSelected.emit($event.data);
  }

  onRowUnselect($event: any) {
    console.log('onRowUnselect');
    this.rowSelected.emit(null);
  }

  @ViewChild('dt1', { static: true }) dt1!: Table;
  selectedItem: any;
  defaultRowsPerPage: number = 10;
  globalFilter: string = '';
  formGroup!: FormGroup;
  filterValues: string[] = [];
  FilterChipsPlaceholder: string = $localize`:@@Placeholders.Search:Search`;

  @Input()
  data: any[] = [];

  @Input()
  cols: any[] = [];

  @Input()
  key: any = '';

  @Input()
  RowsPerPageOptions: any[] = [5, 10, 20, 50];

  @Output()
  rowSelected: EventEmitter<any> = new EventEmitter();

  constructor() {
    if (this.key === '') {
      console.warn('key is empty!');
    }
  }

  ngOnInit(): void {
    this.formGroup = new FormGroup({
      values: new FormControl<string[] | null>(null),
    });
    this.formGroup.get('values')?.valueChanges.subscribe((values) => {
      this.filterValues = values;
      this.filterData(values);
      this.dt1.value = this.filterData(values);
    });

    this.searchOptions = {
      fields: this.cols.map((col) => col.field),
      operator: 'contains',
      ignoreCase: true,
      key: '',
    };

    this.toolbarOptions = ['Search'];
  }

  clear(table: Table) {
    table.clear();
  }

  filterData(values: string[]) {
    const filteredData = this.data.filter(item => {
      for (const value of values) {
        let match = false;

        // Check if the value exists in any property of the item
        for (const key in item) {
          if (item.hasOwnProperty(key) && typeof item[key] === 'string' &&
              item[key].toLowerCase().includes(value.toLowerCase())) {
            match = true;
            break;
          }
        }

        // If any value doesn't match, exclude the item from filtered data
        if (!match) {
          return false;
        }
      }

      // All values matched, include the item in filtered data
      return true;
    });
    return filteredData;
  }
}
