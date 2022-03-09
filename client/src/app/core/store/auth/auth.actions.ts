import { Action } from '@ngrx/store';

export enum AuthActionTypes {
    Register = '[Auth] Register',
    RegisterSuccess = '[Auth] Register Success',
    Login = '[Auth] Login',
    LoginSuccess = '[Auth] Login Success',
    Logout = '[Auth] Logout',
    LogoutSuccess = '[Auth] Logout Success',
    SetAuth = '[Auth] Set Auth',
    SetToken = '[Auth] Set Token',
}

export class Register implements Action {
    public readonly type = AuthActionTypes.Register;
    constructor(public payload: any) { }
}

export class RegisterSuccess implements Action {
    public readonly type = AuthActionTypes.RegisterSuccess;
    constructor(public payload: string) { }
}

export class Login implements Action {
    public readonly type = AuthActionTypes.Login;
    constructor(public payload: any) { }
}

export class LoginSuccess implements Action {
    public readonly type = AuthActionTypes.LoginSuccess;
    constructor(public payload: any) { }
}

export class Logout implements Action {
    public readonly type = AuthActionTypes.Logout;
}

export class LogoutSuccess implements Action {
    public readonly type = AuthActionTypes.LogoutSuccess;
}

export class SetAuth implements Action {
    public readonly type = AuthActionTypes.SetAuth;
    constructor(public payload: any) { }
}

export class SetToken implements Action {
    public readonly type = AuthActionTypes.SetToken;
    constructor(public payload: any) { }
}

export type AuthActions =
    | Register
    | RegisterSuccess
    | Login
    | LoginSuccess
    | Logout
    | LogoutSuccess
    | SetAuth
    | SetToken;
