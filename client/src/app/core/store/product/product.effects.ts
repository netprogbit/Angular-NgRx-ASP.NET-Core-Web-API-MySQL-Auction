import { Injectable } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { Effect, ofType, Actions } from '@ngrx/effects';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { MatSnackBar } from '@angular/material';
import {
    GetProducts,
    GetProductsSuccess,
    SearchProduct,            
    SubmitProduct,
    ChangeProgress,
    SubmitProductSuccess,
    DeleteProduct,    
    ProductActionTypes
} from './product.actions';
import { of } from 'rxjs';
import { 
    switchMap, 
    map, 
    withLatestFrom, 
    debounceTime, 
    distinctUntilChanged, 
    tap 
} from 'rxjs/operators';
import { paginator, searchTerm } from './product.selectors';
import { IAppState } from '../app/app.state';
import { ProductService } from '../../services/product.service';
import { IPaginator } from '../../models/paginator.interface';
import { IProduct } from '../../models/product.interface';

@Injectable()
export class ProductEffects {

    @Effect()
    getProducts$ = this.actions$.pipe(
        ofType<GetProducts>(ProductActionTypes.GetProducts),
        withLatestFrom(            
            this.store.pipe(select(searchTerm)),
            this.store.pipe(select(paginator))
        ),
        switchMap(([payload, searchTerm, paginator]) => {
            return this.productService.getProducts(null, searchTerm, paginator.pageIndex, paginator.pageSize);
        }),
        switchMap((paginator: IPaginator<IProduct>) => {
            return of(new GetProductsSuccess(paginator));
        })
    );

    @Effect()
    searchProduct$ = this.actions$.pipe(
        ofType<SearchProduct>(ProductActionTypes.SearchProduct),
        debounceTime(300),
        distinctUntilChanged(),
        map(action => action.payload),
        withLatestFrom(            
            this.store.pipe(select(paginator))
        ),
        switchMap(([payload, paginator]) => {
            return this.productService.getProducts(null, payload, paginator.pageIndex, paginator.pageSize);
        }),
        switchMap((paginator: IPaginator<IProduct>) => {
            return of(new GetProductsSuccess(paginator));
        })
    );    

    @Effect()
    submitProduct$ = this.actions$.pipe(
        ofType<SubmitProduct>(ProductActionTypes.SubmitProduct),
        map(action => action.payload),
        switchMap(payload => {
            return this.productService.submit(payload.id, payload.categoryName, payload.name, payload.description, payload.price, payload.image);
        }),
        switchMap((event: HttpEvent<any>) => {
            switch (event.type) {
                case HttpEventType.UploadProgress:                    
                    return of(new ChangeProgress(Math.round(event.loaded / event.total * 100)));
                case HttpEventType.Response:                                        
                    return [new SubmitProductSuccess(event.body), new GetProducts()];
                default:
                    return of();
            }
        })
    );

    @Effect({ dispatch: false })
    submitProductSuccess$ = this.actions$.pipe(
        ofType<SubmitProductSuccess>(ProductActionTypes.SubmitProductSuccess),
        map(action => action.payload),        
        tap(payload => {
            this.snackBar.open(payload, 'OK', { duration: 3000 });
        }),
    );

    @Effect()
    deleteProduct$ = this.actions$.pipe(
        ofType<DeleteProduct>(ProductActionTypes.DeleteProduct),
        map(action => action.payload),
        switchMap((id) => {
            return this.productService.deleteProduct(id);
        }),
        tap(data => {
            this.snackBar.open(data, 'OK', { duration: 3000 });
        }),
        switchMap(() => {
            return of(new GetProducts());
        })
    );

    constructor(
        private productService: ProductService,
        private actions$: Actions,
        private store: Store<IAppState>,
        private snackBar: MatSnackBar,
    ) { }
}
