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


  @Input() options: DropdownOption[] = [];
  @Output() selectionChange = new EventEmitter<any>();

  isOpen = false;
  value: any;
  selectedLabel: string = '';

  onChange: any = () => { };
  onTouched: any = () => { };

  toggleDropdown() {
    this.isOpen = !this.isOpen;
  }

  selectOption(option: DropdownOption, event: Event) {
    this.value = option.value;
    this.selectedLabel = option.label;
    this.isOpen = false;
    this.onChange(this.value);
    this.onTouched();
    this.selectionChange.emit(this.value);
  }

  @HostListener('document:click', ['$event'])
  onClickOutside(event: Event) {
    const targetElement = event.target as HTMLElement;
    if (targetElement && !targetElement.closest('.dropdown-container')) {
      this.isOpen = false;
    }
  }

  writeValue(value: any): void {
    this.value = value;
    const selectedOption = this.options.find(option => option.value === value);
    if (selectedOption) {
      this.selectedLabel = selectedOption.label;
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    // Optionally handle the disabled state
  }
}
