// NGXS Actions for ClientState (comparing to CQRS, similar to Commands)

import { Client } from 'src/app/clients/models/client.model';

export class SelectClient {
  static readonly type = '[ClientState] SelectClient';
  constructor(public payload: string) {}
}

export class UnselectClient {
  static readonly type = '[ClientState] UnselectClient';
  constructor() {}
}
