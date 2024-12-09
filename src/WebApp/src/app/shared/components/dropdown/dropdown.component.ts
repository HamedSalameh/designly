import { Component, EventEmitter, forwardRef, HostListener, Input, Output } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

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
export class DropdownComponent implements ControlValueAccessor {
  @Input() data: DropdownOption[] = [];
  @Input() allowFiltering: boolean = true;
  @Input() placeholder: string = '';

  @Output() valueChange = new EventEmitter<any>();

  fields: Object = { text: 'label', value: 'value' };
  private innerValue: any;

  // ControlValueAccessor methods

  // Writes a new value to the element
  writeValue(value: any): void {
    this.innerValue = value;
  }

  // Registers a callback function that is called when the control's value changes
  registerOnChange(fn: (value: any) => void): void {
    this.onChange = fn;
  }

  // Registers a callback function that is called when the control is touched
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  // Optionally sets the disabled state of the control
  setDisabledState?(isDisabled: boolean): void {
    // Handle the disabled state if needed
  }

  // Handle value changes
  onValueChange(event: any) {
    this.innerValue = event.value; // Update internal value
    this.valueChange.emit(this.innerValue); // Emit event
    this.onChange(this.innerValue); // Call registered onChange
  }

  // Handle filtering event
  onFiltering(event: any) {
    const query = event.text.toLowerCase();
    const filteredData = this.data.filter(option => option.label.toLowerCase().includes(query));
    event.updateData(filteredData);
  }

  // Internal methods to keep track of change and touch callbacks
  private onChange: (value: any) => void = () => {};
  private onTouched: () => void = () => {};

  // Optional: Call onTouched when the control is interacted with
  @HostListener('blur')
  onBlur() {
    this.onTouched();
  }
}
