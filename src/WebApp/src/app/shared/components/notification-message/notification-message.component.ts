import { Component, Input } from '@angular/core';

export enum NotificationMessageSeverity {
  ERROR = 'error',
  WARN = 'warning',
  INFO = 'info',
  SUCCESS = 'success'
}

export interface NotificationMessage {
  severity: NotificationMessageSeverity;
  summary: string;
  detail: string;
}

@Component({
    selector: 'app-notification-message',
    templateUrl: './notification-message.component.html',
    styleUrls: ['./notification-message.component.scss'],
    standalone: false
})
export class NotificationMessageComponent {

  @Input() messages: NotificationMessage[] = [];

  public addMessage(message: NotificationMessage): void {
    this.messages.push(message);
  }

  public clearMessages(): void {
    this.messages = [];
  }

}
