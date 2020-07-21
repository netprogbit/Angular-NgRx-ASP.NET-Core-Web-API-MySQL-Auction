import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Params } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { StringHelper } from 'src/app/shared/helpers/string.helper';
import { IAppState } from 'src/app/core/store/app/app.state';
import { Login } from 'src/app/core/store/auth/auth.actions';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {

  public loginForm: FormGroup;  
  private paramsSubscript: Subscription;

  constructor(private store: Store<IAppState>, private snackBar: MatSnackBar, private formBuilder: FormBuilder, private route: ActivatedRoute) { }

  ngOnInit() {
    
    this.loginForm = this.formBuilder.group({                        
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });    
  }

  // Login form controls getter
  get efc() {
    return this.loginForm.controls;
  }

  onSubmit() {
      this.store.dispatch(new Login(this.loginForm.value));          
  }

  ngOnDestroy() {

    if (this.paramsSubscript) {
      this.paramsSubscript.unsubscribe();
    }       
  }
}
