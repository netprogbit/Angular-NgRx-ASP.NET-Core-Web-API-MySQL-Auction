import { Action } from '@ngrx/store';
import { IPaginator } from '../../models/paginator.interface';
import { IUser } from '../../models/user.interface';

export enum UserActionTypes {
    GetUsers = '[User] Get Users',
    GetUsersSuccess = '[User] Get Users Success',        
    ChangePaginator = '[User] Change Paginator',
    SearchUser = '[User] Search User',
    SubmitUser = '[User] Submit User',
    SubmitUserSuccess = '[User] Submit User Success',           
    DeleteUser = '[User] Delete User',
}

export class GetUsers implements Action {
    public readonly type = UserActionTypes.GetUsers;    
}

export class GetUsersSuccess implements Action {
    public readonly type = UserActionTypes.GetUsersSuccess;
    constructor(public payload: IPaginator<IUser>) { }
}

export class ChangePaginator implements Action {
    public readonly type = UserActionTypes.ChangePaginator;
    constructor(public payload: any) { }
}

export class SearchUser implements Action {
    public readonly type = UserActionTypes.SearchUser;
    constructor(public payload: string) { }
}

export class SubmitUser implements Action {
    public readonly type = UserActionTypes.SubmitUser;
    constructor(public payload: any) { }
}
export class SubmitUserSuccess implements Action {
    public readonly type = UserActionTypes.SubmitUserSuccess;
    constructor(public payload: string) { }
}

export class DeleteUser implements Action {
    public readonly type = UserActionTypes.DeleteUser;
    constructor(public payload: number) { }
}

export type UserActions =
    | GetUsers
    | GetUsersSuccess    
    | ChangePaginator
    | SearchUser
    | SubmitUser
    | SubmitUserSuccess           
    | DeleteUser;