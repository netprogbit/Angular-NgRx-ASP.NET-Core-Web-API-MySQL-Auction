import { Action } from '@ngrx/store';
import { IProduct } from '../../models/product.interface';
import { IPaginator } from '../../models/paginator.interface';

export enum ProductActionTypes {
    GetProducts = '[Product] Get Products',
    GetProductsSuccess = '[Product] Get Products Success',
    SearchProduct = '[Product] Search Product',
    ChangePaginator = '[Product] Change Paginator',                    
    SubmitProduct = '[Product] Submit Product',
    SubmitProductSuccess = '[Product] Submit Product Success',    
    ChangeProgress = '[Product] Change Progress',    
    DeleteProduct = '[Product] Delete Product',        
}

export class GetProducts implements Action {
    public readonly type = ProductActionTypes.GetProducts;    
}

export class GetProductsSuccess implements Action {
    public readonly type = ProductActionTypes.GetProductsSuccess;
    constructor(public payload: IPaginator<IProduct>) { }
}

export class SearchProduct implements Action {
    public readonly type = ProductActionTypes.SearchProduct;
    constructor(public payload: string) { }
}

export class ChangePaginator implements Action {
    public readonly type = ProductActionTypes.ChangePaginator;
    constructor(public payload: any) { }
}

export class SubmitProduct implements Action {
    public readonly type = ProductActionTypes.SubmitProduct;
    constructor(public payload: any) { }
}

export class SubmitProductSuccess implements Action {
    public readonly type = ProductActionTypes.SubmitProductSuccess;
    constructor(public payload: string) { }    
}

export class ChangeProgress implements Action {
    public readonly type = ProductActionTypes.ChangeProgress;
    constructor(public payload: number) { }
}

export class DeleteProduct implements Action {
    public readonly type = ProductActionTypes.DeleteProduct;
    constructor(public payload: number) { }
}

export type ProductActions =
    | GetProducts
    | GetProductsSuccess
    | SearchProduct
    | ChangePaginator    
    | SubmitProduct
    | SubmitProductSuccess
    | ChangeProgress    
    | DeleteProduct;
