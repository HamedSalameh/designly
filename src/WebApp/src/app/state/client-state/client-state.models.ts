// the state model
export class ClientStateModel {
  static editMode(editMode: any): import("rxjs").Observable<boolean> {
    throw new Error('Method not implemented.');
  }
  selectedClientId: string | null = null;
  editMode: boolean = false;
  draftEntity: any = null;
}
