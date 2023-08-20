// NGXS Actions for ClientState (comparing to CQRS, similar to Commands)

import { Client } from 'src/app/clients/models/client.model';

export class AddClient {
  static readonly type = '[ClientState] AddClient';
  constructor() {}
}

export class SelectClient {
  static readonly type = '[ClientState] SelectClient';
  constructor(public payload: string) {}
}

export class UnselectClient {
  static readonly type = '[ClientState] UnselectClient';
  constructor() {}
}

// Sets the application in edit mode per the client Id
export class EditMode {
  static readonly type = '[ClientState] EditMode';
  constructor(public payload: string) {}
}

export class ViewMode {
  static readonly type = '[ClientState] CancelEditMode';
  constructor() {}
}