import { createAction, props } from "@ngrx/store";
import { SigninRequest } from "../models/signin-request.model";

export const LOGIN_START = '[Auth] Login Start';
export const LOGIN_SUCCESS = '[Auth] Login Success';
export const LOGIN_FAILED = '[Auth] Login Failed';

export const LOGOUT = '[Auth] Logout';

export const SET_TOKEN = '[Auth] Set Token';
export const REVOKE_TOKEN = '[Auth] Revoke Token';
export const SET_USER = '[Auth] Set User';

export const CLEAR_ERROR = '[Auth] Clear Error';
// Signup is not supported in MVP
//export const SIGNUP_START = '[Auth] Signup Start';
//export const SIGNUP_SUCCESS = '[Auth] Signup Success';
//export const SIGNUP_FAILED = '[Auth] Signup Failed';

// TODO: Implement auto logout
//export const AUTO_LOGIN = '[Auth] Auto Login';
//export const AUTO_LOGOUT = '[Auth] Auto Logout';

export const loginStart = createAction(LOGIN_START, props<{ signInRequest: SigninRequest }>());
export const loginSuccess = createAction(LOGIN_SUCCESS, props<{ user: string; token: string; redirect: boolean }>());
export const loginFailed = createAction(LOGIN_FAILED, props<{ error: string }>());

export const logout = createAction(LOGOUT);

// Authenitcation state actions
export const setToken = createAction(SET_TOKEN, props<{ token: string }>());
export const revokeToken = createAction(REVOKE_TOKEN);
export const setUser = createAction(SET_USER, props<{ user: string }>());