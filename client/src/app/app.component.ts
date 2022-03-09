import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { IAppState } from './core/store/app/app.state';
import { SetAuth } from './core/store/auth/auth.actions';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>'
})
export class AppComponent implements OnInit {

  title = 'client';

  constructor(private store: Store<IAppState>) { }

  ngOnInit() {
    const userId: any = localStorage.getItem('userId');
    const token: any = localStorage.getItem('token');
    const refreshToken: any = localStorage.getItem('refreshToken');
    const jsonRoles: any = localStorage.getItem("roles");    

    if (userId && token && refreshToken && jsonRoles) {
      this.store.dispatch(new SetAuth({ 
        userId: userId, 
        token: token, 
        refreshToken: refreshToken, 
        roles: JSON.parse(jsonRoles) 
      }));
    }
  }
}
