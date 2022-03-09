import { Component, OnInit, Inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IAppState } from 'src/app/core/store/app/app.state';
import { DeleteUser } from 'src/app/core/store/user/user.actions';

@Component({
  selector: 'app-user-delete',
  templateUrl: './user-delete.component.html',
  styleUrls: ['./user-delete.component.css']
})
export class UserDeleteComponent implements OnInit {

  public title: string;  

  constructor(public dialogRef: MatDialogRef<UserDeleteComponent>, private store: Store<IAppState>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {

    this.title = this.data.title;
  }

  onDelete(): void {
    this.store.dispatch(new DeleteUser(this.data.id));
    this.onCancel();    
  }

  onCancel(): void {
    this.dialogRef.close();
  }  

}
