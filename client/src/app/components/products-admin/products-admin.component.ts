import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { MatDialog } from '@angular/material';
import { PriceHelper } from './../../shared/helpers/price.helper';
import { ProductEditComponent } from './../product-edit/product-edit.component';
import { ProductDeleteComponent } from './../product-delete/product-delete.component';
import { IProduct } from './../../core/models/product.interface';
import { Observable } from 'rxjs';
import { IAppState } from 'src/app/core/store/app/app.state';
import { GetProducts, SearchProduct, ChangePaginator } from 'src/app/core/store/product/product.actions';
import { paginator, searchTerm } from 'src/app/core/store/product/product.selectors';
import { IPaginator } from 'src/app/core/models/paginator.interface';

@Component({
  selector: 'app-products-admin',
  templateUrl: './products-admin.component.html',
  styleUrls: ['./products-admin.component.css']
})
export class ProductsAdminComponent implements OnInit {

  public displayedColumns: string[] = ['id', 'categoryName', 'name', 'description', 'price', 'sellerPrice', 'imageFileName', 'bidderEmail', 'actions'];    
  public paginator$: Observable<IPaginator<IProduct>> = this.store.pipe(select(paginator));
  public searchTerm$: Observable<string> = this.store.pipe(select(searchTerm));    

  constructor(public dialog: MatDialog, private store: Store<IAppState>) { }

  ngOnInit() {  
    this.store.dispatch(new GetProducts()); // Getting products    
  }
  
  public searchProduct(searchTerm: string): void {
    this.store.dispatch(new SearchProduct(searchTerm));
  }

  public getProducts(pageIndex: number, pageSize: number): void {
    this.store.dispatch(new ChangePaginator({ pageIndex: pageIndex, pageSize: pageSize }));
    this.store.dispatch(new GetProducts());
  }

  public add(): void {

    this.dialog.open(ProductEditComponent, {
      data: { title: "Adding Product", id: 0 }
    });
  }

  public edit(i: number, id: number, categoryName: string, name: string, description: string, price: number, imageFileName: string): void {

    this.dialog.open(ProductEditComponent, {
      data: { title: "Editing Product", id: id, categoryName: categoryName, name: name, description: description, price: PriceHelper.format(price), imageFileName: imageFileName }
    });
  }

  public delete(i: number, id: number, categoryName: string, name: string, description: string, price: number, imageFileName: string): void {

    this.dialog.open(ProductDeleteComponent, {
      data: { title: "Deleting Product", id: id, categoryName: categoryName, name: name, description: description, price: PriceHelper.format(price), imageFileName: imageFileName }
    });
  }  

}
