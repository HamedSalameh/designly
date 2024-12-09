import { createEntityAdapter, EntityState } from "@ngrx/entity";
import { AuthenticatedUser } from "src/app/authentication/state/auth.state";
import { Member } from "../models/member.model";

export const ACCOUNT_STATE_NAME = 'account';

export interface IAccountState extends EntityState<Member> {
}

export const AccountAdapter = createEntityAdapter<Member>({
  selectId: (user: Member) => user.id,
  sortComparer: false,
});

export const InitialAccountState: IAccountState = AccountAdapter.getInitialState({
});