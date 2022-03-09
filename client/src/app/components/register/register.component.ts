import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Store } from '@ngrx/store';
import { MustMatch } from './../../shared/validators/match-password.validator';
import { IAppState } from 'src/app/core/store/app/app.state';
import { Register } from 'src/app/core/store/auth/auth.actions';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  public registerForm: FormGroup;  

  constructor(private store: Store<IAppState>, private formBuilder: FormBuilder) { }

  ngOnInit() {

    this.registerForm = this.formBuilder.group({
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    },
    {
      validator: MustMatch('password', 'confirmPassword')
    });
  }

  // Register form controls getter
  get efc() {
    return this.registerForm.controls;
  }

  onSubmit() {
    this.store.dispatch(new Register(this.registerForm.value));
  }  

}
