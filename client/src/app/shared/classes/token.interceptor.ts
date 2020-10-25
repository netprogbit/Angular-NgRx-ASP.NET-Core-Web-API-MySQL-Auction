import { Injectable } from '@angular/core'
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http'
import { AuthService } from 'src/app/core/services/auth.service'
import { Router } from '@angular/router'
import { Observable } from 'rxjs'

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    constructor(private authService: AuthService, private router: Router) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        if (this.authService.isAuthenticated()) {
            req = req.clone({
                setHeaders: this.authService.getHeaders()
            })
        }

        return next.handle(req);
    }
}