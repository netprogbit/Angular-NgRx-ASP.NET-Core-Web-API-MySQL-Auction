import { Injectable, OnDestroy } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../../environments/environment';
import { Observable, Subscription } from 'rxjs';
import { IAppState } from '../store/app/app.state';
import { authData } from '../store/auth/auth.selectors';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnDestroy {

  private userId: number;
  private role: string;
  private token: string;
  private tokenSubscript: Subscription;

  constructor(private store: Store<IAppState>, private httpClient: HttpClient) {

    // Subscribe to token notification
    this.tokenSubscript = this.store.pipe(select(authData)).subscribe(authData => {
      this.userId = authData.userId;
      this.role = authData.role;
      this.token = authData.token;
    });
  }

  public register(user: any): Observable<any> {

    const registerUrl = `${environment.apiUrl + environment.apiRegister}`;
    const formData: any = new FormData();
    formData.append("firstName", user.firstName);
    formData.append("lastName", user.lastName);
    formData.append("email", user.email);
    formData.append("password", user.password);
    return this.httpClient.post<any>(registerUrl, formData);
  }

  public login(user: any): Observable<any> {

    const loginUrl = `${environment.apiUrl + environment.apiLogin}`;
    const formData: any = new FormData();
    formData.append("email", user.email);
    formData.append("password", user.password);
    return this.httpClient.post<any>(loginUrl, formData);
  }

  public setAuth(userId: number, role: string, token: string) {
    this.userId = userId;
    this.role = role;
    this.token = token;
  }

  getUserId(): number {
    return this.userId;
  }

  getRole(): string {
    return this.role;
  }

  getToken(): string {
    return this.token
  }

  public isAuthenticated(): boolean {
    return !!this.token;
  }

  public logout() {
    this.userId = 0;
    this.role = null;
    this.token = null;
    localStorage.clear();
  }

  ngOnDestroy() {
    if (this.tokenSubscript) {
      this.tokenSubscript.unsubscribe();
    }
  }
}
