import { createSelector } from '@ngrx/store';
import { IAppState } from '../app/app.state';
import { IAuctionState } from './auction.state';

const auction = (state: IAppState) => state.auction;
export const paginator = createSelector(auction, (state: IAuctionState) => state.paginator);
export const selectedProduct = createSelector(auction, (state: IAuctionState) => state.selectedProduct);
export const categoryName = createSelector(auction, (state: IAuctionState) => state.categoryName);
export const searchTerm = createSelector(auction, (state: IAuctionState) => state.searchTerm);
export const sellerPrice = createSelector(auction, (state: IAuctionState) => state.sellerPrice);
