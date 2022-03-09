import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { CategoryEditComponent } from './../category-edit/category-edit.component';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs';
import { ICategory } from './../../core/models/category.interface';
import { IPaginator } from 'src/app/core/models/paginator.interface';
import { IAppState } from 'src/app/core/store/app/app.state';
import { paginator, searchTerm } from 'src/app/core/store/category/category.selectors';
import { GetCategories, SearchCategory } from 'src/app/core/store/category/category.actions';
import { ChangePaginator } from 'src/app/core/store/category/category.actions';

@Component({
  selector: 'app-categories-admin',
  templateUrl: './categories-admin.component.html',
  styleUrls: ['./categories-admin.component.css']
})
export class CategoriesAdminComponent implements OnInit {

  public displayedColumns: string[] = ['id', 'name', 'imageFileName', 'actions'];
  public paginator$: Observable<IPaginator<ICategory>> = this.store.pipe(select(paginator));
  public searchTerm$: Observable<string> = this.store.pipe(select(searchTerm));

  constructor(public dialog: MatDialog, private store: Store<IAppState>) { }

  ngOnInit() {
    this.store.dispatch(new GetCategories()); // Getting categories    
  }

  public searchCategory(searchTerm: string): void {
    this.store.dispatch(new SearchCategory(searchTerm));
  }

  public getCategories(pageIndex: number, pageSize: number): void {
    this.store.dispatch(new ChangePaginator({ pageIndex: pageIndex, pageSize: pageSize }));
    this.store.dispatch(new GetCategories());
  }

  public add(): void {

    this.dialog.open(CategoryEditComponent, {
      data: { title: "Adding Category", id: 0 }
    });
  }

  public edit(i: number, id: number, name: string, imageFileName: string): void {

    this.dialog.open(CategoryEditComponent, {
      data: { title: "Editing Category", id: id, name: name, imageFileName: imageFileName }
    });
  }  
  
}
