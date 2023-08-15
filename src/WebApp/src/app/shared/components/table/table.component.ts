import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewEncapsulation,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import {
  SearchSettingsModel,
  SelectionSettingsModel,
} from '@syncfusion/ej2-angular-grids';
import { ClickEventArgs } from '@syncfusion/ej2/navigations';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
  encapsulation: ViewEncapsulation.None, // This is needed to override the default encapsulation of Angular
})
export class TableComponent implements OnInit {
  // Syncfusion Grid Search Settings
  defaultRowsPerPage: number = 10;
  public searchOptions?: SearchSettingsModel;
  public toolbarOptions?: any[]; // ToolbarItems[];
  public selectionOptions: SelectionSettingsModel = {
    type: 'Single',
    mode: 'Row',
  };
  public pageSettings: Object = {
    pageSizes: true,
    pageSize: this.defaultRowsPerPage,
  };

  onRowSelect($event: any) {
    this.rowSelected.emit($event.data);
  }

  onRowUnselect($event: any) {
    console.log('onRowUnselect');
    this.rowSelected.emit(null);
  }

  selectedItem: any;
  SearchtextPlaceholder: string = $localize`:@@Placeholders.Search:Search`;
  AddNewClient: string = $localize`:@@Buttons.AddNewClient:Add New Client`;

  @Input()
  data: any[] = [];

  @Input()
  cols: any[] = [];

  @Input()
  key: any = '';

  @Output()
  rowSelected: EventEmitter<any> = new EventEmitter();

  @Output()
  addClient: EventEmitter<any> = new EventEmitter();

  constructor() {
    if (this.key === '') {
      console.warn('key is empty!');
    }
  }

  ngOnInit(): void {
    this.searchOptions = {
      fields: this.cols.map((col) => col.field),
      operator: 'contains',
      ignoreCase: true,
      key: '',
    };

    this.toolbarOptions = [
      'Search',
      {
        text: this.AddNewClient,
        tooltipText: 'Add',
        prefixIcon: 'e-plus',
        id: 'addClientAction',
      },
    ];
  }

  toolbarClickHandler(args: ClickEventArgs) {
    if (args) {
      const action = args.item?.id;
      switch (action) {
        case 'addClientAction':
          this.addClient.emit();
          break;
        default:
          break;
      }
    }
  }
}
