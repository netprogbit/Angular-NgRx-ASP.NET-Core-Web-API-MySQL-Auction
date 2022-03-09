import { HttpHelper } from './../../shared/helpers/http.helper';
import { mergeMap } from 'rxjs/operators';
import { environment } from './../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { IPaginator } from '../models/paginator.interface';
import { IUser } from '../models/user.interface';

@Injectable({
  providedIn: 'root'
})
export class UserService {  

  constructor(private httpClient: HttpClient) { }

  public getUsers(searchTerm: string, pageIndex: number, pageSize: number): Observable<IPaginator<IUser>> {
    const usersUrl = `${environment.apiUrl + environment.apiUsers}`;    
    const params = HttpHelper.getPaginatorParams(null, searchTerm, pageIndex, pageSize);
    return this.httpClient.get<IPaginator<IUser>>(usersUrl, { params: params }).pipe(
      mergeMap(data => {        
        return of(data);
      })
    );
  }

  public submit(id: number, userName: string, email: string): Observable<any> {    
    const userUpdateUrl = `${environment.apiUrl + environment.apiUser}`;
    const body = { id, userName, email };
    return this.httpClient.put(userUpdateUrl, body);
  }

  public deleteUser(id: number): Observable<any> {
    const userDeleteUrl = `${environment.apiUrl + environment.apiUser + id}`;    
    return this.httpClient.delete(userDeleteUrl);
  }

}
