import { Action } from '@ngrx/store';
import { IProduct } from '../../models/product.interface';
import { IPaginator } from '../../models/paginator.interface';

export enum AuctionActionTypes {
    GetProducts = '[Auction] Get Products',
    GetProductsSuccess = '[Auction] Get Products Success',
    SearchProduct = '[Auction] Search Product',
    ChangePaginator = '[Auction] Change Paginator',
    ChangeCategoryName = '[Auction] Change Category Name',
    ChangeSellerPrice = '[Auction] Change Seller Price',
    GetProduct = '[Auction] Get Product',
    GetProductSuccess = '[Auction] Get Product Success',
    BuyProduct = '[Auction] Get Buy Product',
    BuyProductSuccess = '[Auction] Buy Product Success',        
}

export class GetProducts implements Action {
    public readonly type = AuctionActionTypes.GetProducts;    
}

export class GetProductsSuccess implements Action {
    public readonly type = AuctionActionTypes.GetProductsSuccess;
    constructor(public payload: IPaginator<IProduct>) { }
}

export class SearchProduct implements Action {
    public readonly type = AuctionActionTypes.SearchProduct;
    constructor(public payload: string) { }
}

export class ChangePaginator implements Action {
    public readonly type = AuctionActionTypes.ChangePaginator;
    constructor(public payload: any) { }
}

export class ChangeCategoryName implements Action {
    public readonly type = AuctionActionTypes.ChangeCategoryName;
    constructor(public payload: string) { }
}

export class ChangeSellerPrice implements Action {
    public readonly type = AuctionActionTypes.ChangeSellerPrice;
    constructor(public payload: number) { }
}

export class GetProduct implements Action {
    public readonly type = AuctionActionTypes.GetProduct;
    constructor(public payload: number) { }
}

export class GetProductSuccess implements Action {
    public readonly type = AuctionActionTypes.GetProductSuccess;
    constructor(public payload: IProduct) { }
}

export class BuyProduct implements Action {
    public readonly type = AuctionActionTypes.BuyProduct;
    constructor(public payload: any) { }
}

export class BuyProductSuccess implements Action {
    public readonly type = AuctionActionTypes.BuyProductSuccess;
    constructor(public payload: string) { }    
}

export type AuctionActions =
    | GetProducts
    | GetProductsSuccess
    | SearchProduct
    | ChangePaginator
    | ChangeCategoryName
    | ChangeCategoryName
    | ChangeSellerPrice
    | GetProduct
    | GetProductSuccess
    | BuyProduct
    | BuyProductSuccess;
