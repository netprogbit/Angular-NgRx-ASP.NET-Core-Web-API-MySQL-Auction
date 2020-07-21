import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { MatDialog } from '@angular/material';
import { UserEditComponent } from './../user-edit/user-edit.component';
import { UserDeleteComponent } from './../user-delete/user-delete.component';
import { Observable } from 'rxjs';
import { IUser } from './../../core/models/user.interface';
import { IPaginator } from 'src/app/core/models/paginator.interface';
import { IAppState } from 'src/app/core/store/app/app.state';
import { paginator, searchTerm } from 'src/app/core/store/user/user.selectors';
import { GetUsers, SearchUser } from 'src/app/core/store/user/user.actions';
import { ChangePaginator } from 'src/app/core/store/product/product.actions';

@Component({
  selector: 'app-users-admin',
  templateUrl: './users-admin.component.html',
  styleUrls: ['./users-admin.component.css']
})
export class UsersAdminComponent implements OnInit {

  public displayedColumns: string[] = ['id', 'firstName', 'lastName', 'email', 'role', 'actions'];
  public paginator$: Observable<IPaginator<IUser>> = this.store.pipe(select(paginator));
  public searchTerm$: Observable<string> = this.store.pipe(select(searchTerm));

  constructor(public dialog: MatDialog, private store: Store<IAppState>) { }

  ngOnInit() {
    this.store.dispatch(new GetUsers()); // Getting users
  }

  public searchUser(searchTerm: string): void {
    this.store.dispatch(new SearchUser(searchTerm));
  }

  public getUsers(pageIndex: number, pageSize: number): void {
    this.store.dispatch(new ChangePaginator({ pageIndex: pageIndex, pageSize: pageSize }));
    this.store.dispatch(new GetUsers());
  }
  
  public edit(i: number, id: number, firstName: string, lastName: string, email: string, role: string) {

    this.dialog.open(UserEditComponent, {
      data: { title: "Editing User", id: id, firstName: firstName, lastName: lastName, email: email, role: role }
    });
  }

  public delete(i: number, id: number, firstName: string, lastName: string, email: string, role: string) {

    this.dialog.open(UserDeleteComponent, {
      data: { title: "Deleting Category", id: id, firstName: firstName, lastName: lastName, email: email, role: role }
    });
  }
  
}
