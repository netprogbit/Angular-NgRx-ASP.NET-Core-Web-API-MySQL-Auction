import { Injectable } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { Effect, ofType, Actions } from '@ngrx/effects';
import { MatSnackBar } from '@angular/material';
import {
    GetProducts,
    GetProductsSuccess,
    SearchProduct,        
    GetProduct,
    GetProductSuccess,
    BuyProduct,
    BuyProductSuccess,       
    AuctionActionTypes
} from './auction.actions';
import { of } from 'rxjs';
import { 
    switchMap, 
    map, 
    withLatestFrom, 
    debounceTime, 
    distinctUntilChanged, 
    tap 
} from 'rxjs/operators';
import { paginator, searchTerm, categoryName } from './auction.selectors';
import { IAppState } from '../app/app.state';
import { ProductService } from '../../services/product.service';
import { IPaginator } from '../../models/paginator.interface';
import { IProduct } from '../../models/product.interface';

@Injectable()
export class AuctionEffects {

    @Effect()
    getProducts$ = this.actions$.pipe(
        ofType<GetProducts>(AuctionActionTypes.GetProducts),
        withLatestFrom(
            this.store.pipe(select(categoryName)),
            this.store.pipe(select(searchTerm)),
            this.store.pipe(select(paginator))
        ),
        switchMap(([payload, categoryName, searchTerm, paginator]) => {
            return this.productService.getProducts(categoryName, searchTerm, paginator.pageIndex, paginator.pageSize);
        }),
        switchMap((paginator: IPaginator<IProduct>) => {
            return of(new GetProductsSuccess(paginator));
        })
    );

    @Effect()
    searchProduct$ = this.actions$.pipe(
        ofType<SearchProduct>(AuctionActionTypes.SearchProduct),
        debounceTime(300),
        distinctUntilChanged(),
        map(action => action.payload),
        withLatestFrom(
            this.store.pipe(select(categoryName)),
            this.store.pipe(select(paginator))
        ),
        switchMap(([payload, categoryName, paginator]) => {
            return this.productService.getProducts(categoryName, payload, paginator.pageIndex, paginator.pageSize);
        }),
        switchMap((paginator: IPaginator<IProduct>) => {
            return of(new GetProductsSuccess(paginator));
        })
    );

    @Effect()
    getProduct$ = this.actions$.pipe(
        ofType<GetProduct>(AuctionActionTypes.GetProduct),
        map(action => action.payload),
        switchMap((id) => {
            return this.productService.getProduct(id);
        }),
        switchMap((product: IProduct) => {
            return of(new GetProductSuccess(product));
        })
    );

    @Effect()
    buyProduct$ = this.actions$.pipe(
        ofType<BuyProduct>(AuctionActionTypes.BuyProduct),
        map(action => action.payload),
        switchMap((payload) => {
            return this.productService.buyProduct(payload.userId, payload.productId);
        }),
        switchMap((data: any) => {
            return of(new BuyProductSuccess(data));
        })
    );

    @Effect({ dispatch: false })
    buyProductSuccess$ = this.actions$.pipe(
        ofType<BuyProductSuccess>(AuctionActionTypes.BuyProductSuccess),
        map(action => action.payload),        
        tap(payload => {
            this.snackBar.open(payload, 'OK', { duration: 3000 });
        }),
    );
    
    constructor(
        private productService: ProductService,
        private actions$: Actions,
        private store: Store<IAppState>,
        private snackBar: MatSnackBar,
    ) { }
}
