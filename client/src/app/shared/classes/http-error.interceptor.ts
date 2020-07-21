import { Injectable } from '@angular/core'
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http'
import { Router } from '@angular/router'
import { Observable, throwError } from 'rxjs'
import { catchError } from 'rxjs/operators'
import { MatSnackBar } from '@angular/material'
import { ErrorService } from 'src/app/core/services/error.service'
import { StringHelper } from '../helpers/string.helper'

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

    constructor(private snackBar: MatSnackBar, private errorService: ErrorService, private router: Router) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        return next.handle(req).pipe(
            catchError((error: HttpErrorResponse) => this.handleHttpError(error))
        );
    }

    private handleHttpError(error: HttpErrorResponse): Observable<any> {

        this.errorService.log(`${error.message} Status code: ${error.status.toString()}`).subscribe();

        if (error.status === 401) {
            this.router.navigate(['/login']);
        }
        else {
            let message: string = `Http Error. ${error.statusText}. ${StringHelper.IT_WILL_BE_FIXED}`;

            if (error.error.message)
                message = error.error.message;

            this.snackBar.open(message, 'Dismiss', { duration: 3000 });
        }

        return throwError(error);
    }
}