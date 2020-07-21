import { routerReducer } from '@ngrx/router-store';
import { ActionReducerMap } from '@ngrx/store';
import { IAppState } from './app.state';
import { authReducer } from '../auth/auth.reducer';
import { productReducer } from '../product/product.reducer';
import { categoryReducer } from '../category/category.reducer';
import { userReducer } from '../user/user.reducer';
import { auctionReducer } from '../auction/auction.reducer';

export const appReducers: ActionReducerMap<IAppState> = {
    router: routerReducer,
    auth: authReducer,
    auction: auctionReducer,
    product: productReducer,
    category: categoryReducer,
    user: userReducer,    
};
