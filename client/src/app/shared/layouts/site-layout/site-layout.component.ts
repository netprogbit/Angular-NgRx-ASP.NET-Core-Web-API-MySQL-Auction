import { Router } from '@angular/router';
import { AuthService } from './../../../core/services/auth.service';
import { Component, OnInit } from '@angular/core';
import { NgxPermissionsService } from 'ngx-permissions';
import { Store } from '@ngrx/store';
import { IAppState } from 'src/app/core/store/app/app.state';
import { Logout } from 'src/app/core/store/auth/auth.actions';

@Component({
  selector: 'app-site-layout',
  templateUrl: './site-layout.component.html',
  styleUrls: ['./site-layout.component.css']
})
export class SiteLayoutComponent implements OnInit {

  constructor(private store: Store<IAppState>, private authService: AuthService, private router: Router, private permissionsService: NgxPermissionsService) { }

  ngOnInit() { 
    const roles = this.authService.getRoles();           
    this.permissionsService.loadPermissions(roles);
  }

  logout() {
    this.store.dispatch(new Logout());       
  }
}
