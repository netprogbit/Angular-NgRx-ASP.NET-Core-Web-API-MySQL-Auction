import { Injectable } from '@angular/core'
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http'
import { Router } from '@angular/router'
import { BehaviorSubject, Observable, Subscription, throwError } from 'rxjs'
import { catchError, filter, switchMap, take } from 'rxjs/operators'
import { MatSnackBar } from '@angular/material'
import { ErrorService } from 'src/app/core/services/error.service'
import { StringHelper } from '../helpers/string.helper'
import { Store } from '@ngrx/store'
import { IAppState } from 'src/app/core/store/app/app.state'
import { AuthService } from 'src/app/core/services/auth.service'
import { LogoutSuccess, SetToken } from 'src/app/core/store/auth/auth.actions'
import { environment } from 'src/environments/environment'

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

    private isTokenRefreshing: boolean = false;
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

    constructor(private snackBar: MatSnackBar, private store: Store<IAppState>, private authService: AuthService,
        private errorService: ErrorService, private router: Router) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        req = this.addHeaders(req);

        return next.handle(req).pipe(
            catchError((error: HttpErrorResponse) => {

                if (error.status === 401) {
                    return this.handle401Error(error, req, next);
                } else {
                    return this.handleError(error);
                }
            })
        );
    }

    private handle401Error(error: HttpErrorResponse, req: HttpRequest<any>, next: HttpHandler): Observable<any> {

        if (!this.isTokenRefreshing) {
            this.isTokenRefreshing = true;
            this.refreshTokenSubject.next(null);
            return this.authService.refreshAccessToken().pipe(
                switchMap((auth: any) => {
                    this.isTokenRefreshing = false;

                    if (!auth) {
                        this.store.dispatch(new LogoutSuccess());
                        this.router.navigate(['/login']);
                        return throwError(error);
                    }

                    this.setToken(auth);
                    this.refreshTokenSubject.next(auth.token);
                    return next.handle(this.addHeaders(req, auth.token));
                })
            );
        }
        
        return this.refreshTokenSubject.pipe(
            filter(token => token != null),
            take(1),
            switchMap(token => {
                return next.handle(this.addHeaders(req, token));
            })
        );
    }

    private handleError(error: HttpErrorResponse) {
        const errorLogSubscript: Subscription = this.errorService.log(`${error.message} Status code: ${error.status.toString()}`).subscribe(() => errorLogSubscript.unsubscribe());
        let message: string = `Http Error. ${error.statusText}. ${StringHelper.IT_WILL_BE_FIXED}`;

        if (error.error && !error.error.type) {
            message = error.error;
        }

        this.snackBar.open(message, 'Dismiss', { duration: 3000 });
        return throwError(error);
    }

    private addHeaders(req: HttpRequest<any>, accessToken?: string): HttpRequest<any> {

        let headers = this.authService.getHeaders(accessToken);

        return req.clone({
            setHeaders: headers
        });
    }

    private setToken(auth: any): void {
        localStorage.setItem('token', auth.token);
        this.store.dispatch(new SetToken(auth));
    }
}