import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { environment } from './../../../environments/environment';
import { IProduct } from './../../core/models/product.interface';
import { Observable } from 'rxjs';
import { ICategory } from './../../core/models/category.interface';
import { StringHelper } from './../../shared/helpers/string.helper';
import { IPaginator } from 'src/app/core/models/paginator.interface';
import { IAppState } from 'src/app/core/store/app/app.state';
import { paginator, searchTerm } from 'src/app/core/store/auction/auction.selectors';
import { SearchProduct, GetProducts, ChangePaginator, ChangeCategoryName } from 'src/app/core/store/auction/auction.actions';
import { SelectCategory, GetAllCategories } from 'src/app/core/store/category/category.actions';
import { selectedCategory, allCategories } from 'src/app/core/store/category/category.selectors';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {

  private readonly firstCategory: ICategory = { id: 0, name: StringHelper.ALL, imageFileName: '' };
  public paginator$: Observable<IPaginator<IProduct>> = this.store.pipe(select(paginator));
  public categories$: Observable<ICategory[]> = this.store.pipe(select(allCategories));
  public selectedCategory$: Observable<ICategory> = this.store.pipe(select(selectedCategory));
  public searchTerm$: Observable<string> = this.store.pipe(select(searchTerm));  
  public imageUrl: any = `${environment.apiUrl + environment.imgFolder}`;
      
  constructor(private store: Store<IAppState>) { }

  ngOnInit() {
    this.store.dispatch(new GetAllCategories(this.firstCategory));                
    this.store.dispatch(new SelectCategory(this.firstCategory));    
    this.store.dispatch(new GetProducts());   
  }

  public selectCategory(category): void {
    this.store.dispatch(new ChangeCategoryName(category.name));
    this.store.dispatch(new GetProducts());
  }

  public searchProduct(searchTerm: string): void {
    this.store.dispatch(new SearchProduct(searchTerm));
  }

  public getProducts(pageIndex: number, pageSize: number): void {
    this.store.dispatch(new ChangePaginator({ pageIndex: pageIndex, pageSize: pageSize }));
    this.store.dispatch(new GetProducts());
  }

}
