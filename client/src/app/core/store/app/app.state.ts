import { RouterReducerState } from '@ngrx/router-store';
import { IAuthState, initialAuthState } from '../auth/auth.state';
import { IAuctionState, initialAuctionState } from '../auction/auction.state';
import { IProductState, initialProductState } from '../product/product.state';
import { ICategoryState, initialCategoryState } from '../category/category.state';
import { IUserState, initialUserState } from '../user/user.state';

export interface IAppState {
    router?: RouterReducerState;
    auth: IAuthState,
    auction: IAuctionState,
    product: IProductState;
    category: ICategoryState;
    user: IUserState;    
}

export const initialAppState: IAppState = {
    auth: initialAuthState,
    auction: initialAuctionState,
    product: initialProductState,
    category: initialCategoryState,
    user: initialUserState,    
};

export function getInitialState(): IAppState {
    return initialAppState;
}
