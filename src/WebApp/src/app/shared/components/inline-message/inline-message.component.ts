import { Component, Input } from '@angular/core';

export enum InlineMessageSeverity {
  ERROR = 'error',
  WARN = 'warning',
  INFO = 'info',
  SUCCESS = 'success'
}

export interface InlineMessage {
  severity: InlineMessageSeverity;
  summary: string;
  detail: string;
}

@Component({
  selector: 'app-inline-message',
  templateUrl: './inline-message.component.html',
  styleUrls: ['./inline-message.component.scss']
})
export class InlineMessageComponent {

  @Input() messages: InlineMessage[] = [];

  public addMessage(message: InlineMessage): void {
    this.messages.push(message);
  }

  public clearMessages(): void {
    this.messages = [];
  }

}
