import { Effect, ofType, Actions } from '@ngrx/effects';
import { Injectable } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { MatSnackBar } from '@angular/material';
import {
    GetUsers,
    GetUsersSuccess,
    SearchUser,            
    SubmitUser,
    SubmitUserSuccess,    
    DeleteUser,
    UserActionTypes
} from './user.actions';
import { of } from 'rxjs';
import { 
    switchMap, 
    map, 
    withLatestFrom, 
    debounceTime, 
    distinctUntilChanged, 
    tap 
} from 'rxjs/operators';
import { paginator, searchTerm } from './user.selectors';
import { IAppState } from '../app/app.state';
import { UserService } from '../../services/user.service';
import { IPaginator } from '../../models/paginator.interface';
import { IUser } from '../../models/user.interface';

@Injectable()
export class UserEffects {

    @Effect()
    getUsers$ = this.actions$.pipe(
        ofType<GetUsers>(UserActionTypes.GetUsers),
        withLatestFrom(            
            this.store.pipe(select(searchTerm)),
            this.store.pipe(select(paginator))
        ),
        switchMap(([payload, searchTerm, paginator]) => {
            return this.userService.getUsers(searchTerm, paginator.pageIndex, paginator.pageSize);
        }),
        switchMap((paginator: IPaginator<IUser>) => {
            return of(new GetUsersSuccess(paginator));
        })
    );

    @Effect()
    searchUser$ = this.actions$.pipe(
        ofType<SearchUser>(UserActionTypes.SearchUser),
        debounceTime(300),
        distinctUntilChanged(),
        map(action => action.payload),
        withLatestFrom(            
            this.store.pipe(select(paginator))
        ),
        switchMap(([payload, paginator]) => {
            return this.userService.getUsers(payload, paginator.pageIndex, paginator.pageSize);
        }),
        switchMap((paginator: IPaginator<IUser>) => {
            return of(new GetUsersSuccess(paginator));
        })
    );
    
    @Effect()
    submitUser$ = this.actions$.pipe(
        ofType<SubmitUser>(UserActionTypes.SubmitUser),
        map(action => action.payload),
        switchMap(payload => {
            return this.userService.submit(payload.id, payload.userName, payload.email);
        }),
        switchMap((data: any) => {
            return [new SubmitUserSuccess(data), new GetUsers()];
        })
    );

    @Effect({ dispatch: false })
    submitUserSuccess$ = this.actions$.pipe(
        ofType<SubmitUserSuccess>(UserActionTypes.SubmitUserSuccess),
        map(action => action.payload),        
        tap(payload => {
            this.snackBar.open(payload, 'OK', { duration: 3000 });
        }),
    );

    @Effect()
    deleteProduct$ = this.actions$.pipe(
        ofType<DeleteUser>(UserActionTypes.DeleteUser),
        map(action => action.payload),
        switchMap((id) => {
            return this.userService.deleteUser(id);
        }),
        tap(data => {
            this.snackBar.open(data, 'OK', { duration: 3000 });
        }),
        switchMap(() => {
            return of(new GetUsers());
        })
    );

    constructor(
        private userService: UserService,
        private actions$: Actions,
        private store: Store<IAppState>,
        private snackBar: MatSnackBar,
    ) { }
}
