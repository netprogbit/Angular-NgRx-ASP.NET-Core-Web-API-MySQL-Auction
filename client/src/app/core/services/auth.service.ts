import { Injectable, OnDestroy } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../../environments/environment';
import { Observable, Subscription } from 'rxjs';
import { IAppState } from '../store/app/app.state';
import { authData } from '../store/auth/auth.selectors';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnDestroy {

  private userId: string;
  private token: string;
  private refreshToken: string;
  private roles: string[];
  private tokenSubscript: Subscription;

  constructor(private store: Store<IAppState>, private httpClient: HttpClient) {

    this.tokenSubscript = this.store.pipe(select(authData)).subscribe(authData => {
      this.userId = authData.userId;
      this.token = authData.token;
      this.refreshToken = authData.refreshToken;
      this.roles = authData.roles;
    });
  }

  public register(user: any): Observable<any> {
    const url = `${environment.apiUrl + environment.apiRegister}`;
    const body = { userName: user.userName, email: user.email, password: user.password };
    return this.httpClient.post<any>(url, body);
  }

  public login(user: any): Observable<any> {
    const url = `${environment.apiUrl + environment.apiLogin}`;
    const body = { email: user.email, password: user.password };
    return this.httpClient.post<any>(url, body);
  }

  public refreshAccessToken(): Observable<any> {
    const url = `${environment.apiUrl + environment.apiRefreshToken}`;
    const body = { refreshToken: this.refreshToken };
    return this.httpClient.post<any>(url, body);
  }

  public revokeToken(): Observable<any> {
    const url = `${environment.apiUrl + environment.apiRevokeToken}`;
    const body = { refreshToken: this.refreshToken };
    return this.httpClient.post<any>(url, body);
  }

  public getUserId(): string {
    return this.userId;
  }

  public getRoles(): string[] {
    return this.roles;
  }

  public getHeaders(accessToken?: string): any {
    let token = this.token;

    if (accessToken) {
      token = accessToken;
    }

    return {
      'Authorization': `Bearer ${token}`,
      'enctype': 'multipart/form-data',
      'Accept': 'application/json'
    };
  }

  public isAuthenticated(): boolean {
    return !!this.token;
  }

  ngOnDestroy() {
    if (this.tokenSubscript) {
      this.tokenSubscript.unsubscribe();
    }
  }
}
