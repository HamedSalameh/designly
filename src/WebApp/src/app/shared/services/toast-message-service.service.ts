import { Injectable } from '@angular/core';
import { ToastComponent } from '../components/toast/toast.component';

@Injectable({
  providedIn: 'root',
})
export class ToastMessageService {
  private toastComponent: ToastComponent | undefined;

  constructor() {}

  setToastComponent(component: ToastComponent): void {
    this.toastComponent = component;
  }

  showSuccess(message: string, title?: string) {
    this.toastComponent?.showSuccess(message, title);
  }

  showInfo(message: string, title?: string) {
    this.toastComponent?.showInfo(message, title);
  }

  showWarn(message: string, title?: string) {
    this.toastComponent?.showWarn(message, title);
  }

  showError(message: string, title?: string) {
    this.toastComponent?.showError(message, title);
  }
}
