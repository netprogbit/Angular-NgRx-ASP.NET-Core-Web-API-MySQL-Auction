import { Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate, CanActivateChild {

  constructor(private router: Router, private authService: AuthService) { }

  // Alows only administrator
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {

    const role: string = this.authService.getRole();

    if (role === 'admin') {
      return of(true);
    }
    else {
      this.router.navigate(['/login']);
      return of(false);
    }
  }

  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.canActivate(route, state)
  }
}