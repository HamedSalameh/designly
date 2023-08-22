import { Component, Input, ViewChild } from '@angular/core';
import { ToastMessageService } from '../../services/toast-message-service.service';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss'],
})
export class ToastComponent {
  @ViewChild('element') public element: any;
  public position = { X: 'Left', Y: 'Top' };

  @Input() message: string = '';
  @Input() title: string = '';

  constructor(private toastService: ToastMessageService) { }

  ngOnInit(): void {
    this.toastService.setToastComponent(this);
  }

  showSuccess(message: string, title?: string) {
    const toast = {
      title: title || 'Success',
      content: message,
      cssClass: 'e-toast-success',
      icon: 'circle-check toast-icons',
    };
    this.element.show(toast);
  }

  showInfo(message: string, title?: string) {
    const toast = {
      title: title || 'Info',
      content: message,
      cssClass: 'e-toast-info',
      icon: 'e-circle-info toast-icons',
    };
    this.element.show(toast);
  }

  showWarn(message: string, title?: string) {
    const toast = {
      title: title || 'Warning',
      content: message,
      cssClass: 'e-toast-warning',
      icon: 'e-warning toast-icons',
    };
    this.element.show(toast);
  }

  showError(message: string, title?: string) {
    const toast = {
      title: title || 'Error',
      content: message,
      cssClass: 'e-toast-danger',
      icon: 'e-close toast-icons',
    };
    this.element.show(toast);
  }
  
}
