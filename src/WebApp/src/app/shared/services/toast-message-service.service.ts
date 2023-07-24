import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root',
})
export class ToastMessageService {
  constructor(private messageService: MessageService) {}

  // shows toast message wrapping Prmieng's MessageService
  // https://www.primefaces.org/primeng/showcase/#/toast
  // https://www.primefaces.org/primeng/showcase/#/message
  // https://www.primefaces.org/primeng/showcase/#/messageService

  showSuccess(message: string, title?: string) {
    this.messageService.add({
      severity: 'success',
      summary: title || 'Success',
      detail: message,
    });
  }

  showInfo(message: string, title?: string) {
    this.messageService.add({
      severity: 'info',
      summary: title || 'Info',
      detail: message,
    });
  }

  showWarn(message: string, title?: string) {
    this.messageService.add({
      severity: 'warn',
      summary: title || 'Warn',
      detail: message,
    });
  }

  showError(message: string, title?: string) {
    this.messageService.add({
      severity: 'error',
      summary: title || 'Error',
      detail: message,
    });
  }
}
