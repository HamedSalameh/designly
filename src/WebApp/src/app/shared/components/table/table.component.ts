import {
  Component,
  ContentChild,
  EventEmitter,
  Input,
  OnInit,
  Output,
  TemplateRef,
  ViewEncapsulation,
} from '@angular/core';
import {
  DetailRowService,
  FilterService,
  SearchSettingsModel,
  SelectionSettingsModel,
  SortService,
  ToolbarService,
} from '@syncfusion/ej2-angular-grids';
import { ClickEventArgs } from '@syncfusion/ej2/navigations';

@Component({
    selector: 'app-table',
    templateUrl: './table.component.html',
    styleUrls: ['./table.component.scss'],
    encapsulation: ViewEncapsulation.None, // This is needed to override the default encapsulation of Angular
    providers: [DetailRowService, SortService, FilterService, ToolbarService],
    standalone: false
})
export class TableComponent implements OnInit {

  @ContentChild('detailTemplate', { static: false }) detailTemplate!: TemplateRef<any>;
  
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

  private toolbarActions: Map<string, Function> = new Map();

  onRowSelect($event: any) {
    this.rowSelected.emit($event.data);
  }

  onRowUnselect($event: any) {
    console.log('onRowUnselect');
    this.rowSelected.emit(null);
  }

  selectedItem: any;
  
  @Input()
  data: any[] = [];

  @Input()
  cols: any[] = [];

  @Input()
  toolbarItems: any[] = [];

  @Output()
  rowSelected: EventEmitter<any> = new EventEmitter();

  @Output()
  addClient: EventEmitter<any> = new EventEmitter();

  ngOnInit(): void {

    if (this.toolbarItems) {
      this.toolbarOptions = this.toolbarItems.map((item) => {
        return {
          text: item.text,
          tooltipText: item.tooltipText,
          prefixIcon: item.prefixIcon,
          id: item.id
        };
      });
    }

    this.searchOptions = {
      fields: this.cols.map((col) => col.field),
      operator: 'contains',
      ignoreCase: true,
      key: '',
    };

  }

  toolbarClickHandler(args: ClickEventArgs) {
    const toolbarActionId = args.item?.id;
    if (toolbarActionId) {
      if (toolbarActionId === 'addClientAction') {
        this.addClient.emit();
      }
      // search
      if (toolbarActionId === 'searchClientAction') {
        // Reset search key to clear search results
        this.searchOptions = {
          ...this.searchOptions,
          key: '',
        };
      }
    }
  }
}
