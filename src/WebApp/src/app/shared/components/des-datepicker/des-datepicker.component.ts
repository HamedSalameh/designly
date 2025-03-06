import { Component, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
    selector: 'app-des-datepicker',
    template: `
    <ejs-datepicker [value]="value" (change)="onValueChange($event)" [format]="format">
    </ejs-datepicker>
  `,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => DesDatepickerComponent),
            multi: true
        }
    ],
    standalone: false
})
export class DesDatepickerComponent implements ControlValueAccessor {
  @Input() format: string = 'dd/MM/yyyy'; // Default format
  value: Date | null = null;

  // This is called when the form control value is set or changed.
  onChange = (value: any) => {};

  // This is called when the form control is touched.
  onTouched = () => {};

  // Set the value from the parent form
  writeValue(value: any): void {
    this.value = value;
  }

  // Register the change handler for the form control
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  // Register the touched handler for the form control
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  // Handle disabled state
  setDisabledState?(isDisabled: boolean): void {
    // Handle the component's disabled state
  }

  // Event handler for when the value changes
  onValueChange(event: any): void {
    const value = event.value;
    this.value = value;
    this.onChange(value); // Propagate change to the form control
  }
}
