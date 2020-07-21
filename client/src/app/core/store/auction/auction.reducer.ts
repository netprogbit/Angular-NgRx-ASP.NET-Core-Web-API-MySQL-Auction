import { initialAuctionState, IAuctionState } from './auction.state';
import { AuctionActions, AuctionActionTypes } from './auction.actions';

export const auctionReducer = (state = initialAuctionState, action: AuctionActions): IAuctionState => {
    switch (action.type) {
        case AuctionActionTypes.GetProductsSuccess: {
            return {
                ...state,
                paginator: {
                    ...state.paginator,
                    items: action.payload.items,
                    length: action.payload.length,
                },
            };
        }
        case AuctionActionTypes.GetProductSuccess: {
            return {
                ...state,
                selectedProduct: action.payload,
                sellerPrice: action.payload.sellerPrice,
            };
        }
        case AuctionActionTypes.SearchProduct: {
            return {
                ...state,
                searchTerm: action.payload,
                paginator: {
                    ...state.paginator,
                    pageIndex: 0,
                },
            };
        }
        case AuctionActionTypes.ChangePaginator: {
            return {
                ...state,
                paginator: {
                    ...state.paginator,
                    pageIndex: action.payload.pageIndex,
                    pageSize: action.payload.pageSize,
                },
            };
        }
        case AuctionActionTypes.ChangeCategoryName: {
            return {
                ...state,
                categoryName: action.payload,
                paginator: {
                    ...state.paginator,
                    pageIndex: 0,
                },
            };
        }
        case AuctionActionTypes.ChangeSellerPrice: {
            return {
                ...state,
                sellerPrice: action.payload,
            };
        }                       
        default:
            return state;
    }
};
