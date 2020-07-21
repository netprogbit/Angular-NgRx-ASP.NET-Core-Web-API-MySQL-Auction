import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpBackend, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  private httpWithoutInterceptor: HttpClient;

  constructor(private authService: AuthService, private httpBackend: HttpBackend) {
    this.httpWithoutInterceptor = new HttpClient(httpBackend); // It prevents interceptor looping if log method will throws error
  }

  public log(message: string): Observable<any> {

    const formData: any = new FormData();
    formData.append("message", message);    
    const errorUrl = `${environment.apiUrl + environment.apiErrorLog}`;
    // Headers are required bacause http request without interceptor
    const headers = new HttpHeaders({
      'Authorization': 'Bearer ' + this.authService.getToken(),
      'Accept': 'application/json'
    }); 
    return this.httpWithoutInterceptor.post(errorUrl, formData, { headers: headers });
  }
}
