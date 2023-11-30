import { createAction, props } from "@ngrx/store";
import { SigninRequest } from "../models/signin-request.model";

export const LOGIN_START = '[Auth] Login Start';
export const LOGIN_SUCCESS = '[Auth] Login Success';
export const LOGIN_FAILED = '[Auth] Login Failed';

export const LOGOUT = '[Auth] Logout';

export const CLEAR_ERROR = '[Auth] Clear Error';
// Signup is not supported in MVP
//export const SIGNUP_START = '[Auth] Signup Start';
//export const SIGNUP_SUCCESS = '[Auth] Signup Success';
//export const SIGNUP_FAILED = '[Auth] Signup Failed';

// TODO: Implement auto logout
//export const AUTO_LOGIN = '[Auth] Auto Login';
//export const AUTO_LOGOUT = '[Auth] Auto Logout';

export const loginStart = createAction(LOGIN_START, props<{ signInRequest: SigninRequest }>());
export const loginSuccess = createAction(LOGIN_SUCCESS, props<{ 
    user: string,
    accessToken: string,
    idToken: string,
    expiresIn: string,
    expiresAt: any,
    redirect: boolean }>());
export const loginFailed = createAction(LOGIN_FAILED, props<{ error: string }>());

export const logout = createAction(LOGOUT);
