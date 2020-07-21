import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IAppState } from 'src/app/core/store/app/app.state';
import { SubmitUser } from 'src/app/core/store/user/user.actions';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {

  public editForm: FormGroup;
  public title: string;    

  constructor(private store: Store<IAppState>, private formBuilder: FormBuilder, 
    public dialogRef: MatDialogRef<UserEditComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {

    this.title = this.data.title;

    this.editForm = this.formBuilder.group({
      id: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required],
      role: ['', Validators.required]
    });

    this.editForm.get('id').setValue(this.data.id);
    this.editForm.get('firstName').setValue(this.data.firstName);
    this.editForm.get('lastName').setValue(this.data.lastName);
    this.editForm.get('email').setValue(this.data.email);
    this.editForm.get('role').setValue(this.data.role);    
  }

  // Editing form controls getter
  get efc() {
    return this.editForm.controls;
  }

  public onSubmit(): void {    
    this.store.dispatch(new SubmitUser({
      id: this.editForm.value.id,
      firstName: this.editForm.value.firstName,
      lastName: this.editForm.value.lastName,
      email: this.editForm.value.email,
      role: this.editForm.value.role,
    }));

    this.onCancel();
  }

  public onCancel(): void {
    this.dialogRef.close();
  }  

}
