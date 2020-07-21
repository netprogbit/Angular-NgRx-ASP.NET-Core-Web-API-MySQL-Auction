import { createSelector } from '@ngrx/store';
import { IAppState } from '../app/app.state';
import { IProductState } from './product.state';

const product = (state: IAppState) => state.product;
export const paginator = createSelector(product, (state: IProductState) => state.paginator);
export const searchTerm = createSelector(product, (state: IProductState) => state.searchTerm);
export const progress = createSelector(product, (state: IProductState) => state.progress);
