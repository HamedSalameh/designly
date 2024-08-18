export const ACCOUNT_STATE_NAME = 'account';

export interface IAccountState {
  accountId: string | null;
  // account users
  accountUsers: any[];
}

export const InitialAccountState: IAccountState = {
  accountId: null,
  accountUsers: []
};