import { Effect, ofType, Actions } from '@ngrx/effects';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import {
    Register,
    RegisterSuccess,
    Login,
    LoginSuccess,
    AuthActionTypes,
    Logout,
    LogoutSuccess,
} from './auth.actions';
import { of } from 'rxjs';
import { switchMap, map, tap } from 'rxjs/operators';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthEffects {

    @Effect()
    register$ = this.actions$.pipe(
        ofType<Register>(AuthActionTypes.Register),
        map(action => action.payload),
        switchMap((payload) => {
            return this.authService.register(payload);
        }),        
        switchMap((data) => {
            return of(new RegisterSuccess(data));
        })
    );

    @Effect({ dispatch: false })
    registerSuccess$ = this.actions$.pipe(
        ofType<RegisterSuccess>(AuthActionTypes.RegisterSuccess),
        map(action => action.payload),        
        tap((payload) => {
            this.router.navigate(['/login']);
            this.snackBar.open(payload, 'OK', { duration: 3000 });
        })
    );

    @Effect()
    login$ = this.actions$.pipe(
        ofType<Login>(AuthActionTypes.Login),
        map(action => action.payload),
        switchMap((payload) => {
            return this.authService.login(payload);
        }),
        switchMap((data: any) => {
            return of(new LoginSuccess(data));
        })
    );

    @Effect({ dispatch: false })
    loginSuccess$ = this.actions$.pipe(
        ofType<LoginSuccess>(AuthActionTypes.LoginSuccess),
        map(action => action.payload),
        tap((payload) => {
            localStorage.setItem('userId', payload.userId);
            localStorage.setItem('token', payload.token);
            localStorage.setItem('refreshToken', payload.refreshToken);
            localStorage.setItem("roles", JSON.stringify(payload.roles));
            this.router.navigate(['/products']);
        })
    );

    @Effect()
    logout$ = this.actions$.pipe(
        ofType<Logout>(AuthActionTypes.Logout),
        switchMap(() => {
            return this.authService.revokeToken();
        }),
        switchMap((data) => {
            this.snackBar.open(data, 'OK', { duration: 3000 });
            return of(new LogoutSuccess());
        })
    );

    @Effect({ dispatch: false })
    logoutSuccess$ = this.actions$.pipe(
        ofType<LogoutSuccess>(AuthActionTypes.LogoutSuccess),
        tap(() => {
            localStorage.removeItem('userId');
            localStorage.removeItem('token');
            localStorage.removeItem('refreshToken');
            localStorage.removeItem("roles");
            this.router.navigate(['/login']);
        })
    );

    constructor(
        private authService: AuthService,
        private actions$: Actions,
        private router: Router,
        private snackBar: MatSnackBar,
    ) { }
}
