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
      userName: ['', Validators.required],
      email: ['', Validators.required],
    });

    this.editForm.get('id').setValue(this.data.id);
    this.editForm.get('userName').setValue(this.data.userName);
    this.editForm.get('email').setValue(this.data.email);
  }

  // Editing form controls getter
  get efc() {
    return this.editForm.controls;
  }

  public onSubmit(): void {    
    this.store.dispatch(new SubmitUser({
      id: this.editForm.value.id,
      userName: this.editForm.value.userName,
      email: this.editForm.value.email,
    }));

    this.onCancel();
  }

  public onCancel(): void {
    this.dialogRef.close();
  }  

}
