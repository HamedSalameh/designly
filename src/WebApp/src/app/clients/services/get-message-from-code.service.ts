import { Injectable } from '@angular/core';
import { Strings } from 'src/app/shared/strings';

@Injectable({
  providedIn: 'root'
})
export class GetMessageFromCodeService {

  constructor() { }

  msg = '';

  public getMessageFromCode(code: string): string {
    let message = '';
    switch (code) {
      case 'S001':
        message = Strings.messageFromCodeS001;
        break;
      case 'S002':
        message = Strings.messageFromCodeS002;
        break;
      default:
        message = 'Invalid';
        break;
    };
    return message;
  }
}
