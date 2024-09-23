import { Component, EventEmitter, forwardRef, HostListener, Input, Output } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

interface DropdownOption {
  label: string;
  value: any;
}

@Component({
  selector: 'app-dropdown',
  templateUrl: './dropdown.component.html',
  styleUrls: ['./dropdown.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DropdownComponent),
      multi: true
    }
  ]
})
export class DropdownComponent {

  @Input() data: DropdownOption[] = [];
  @Input() allowFiltering: boolean = true;
  @Input() placeholder: string = '';

  @Output() valueChange = new EventEmitter<any>();

  fields: Object = { text: 'label', value: 'value' };

  onValueChange(event: any) {
    this.valueChange.emit(event.value);
  }

  // Handle filtering event
  onFiltering(event: any) {
    const query = event.text.toLowerCase();
    const filteredData = this.data.filter(option => option.label.toLowerCase().includes(query));
    event.updateData(filteredData);
  }
}