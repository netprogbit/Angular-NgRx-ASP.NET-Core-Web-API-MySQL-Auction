import { initialProductState, IProductState } from './product.state';
import { ProductActions, ProductActionTypes } from './product.actions';

export const productReducer = (state = initialProductState, action: ProductActions): IProductState => {
    switch (action.type) {
        case ProductActionTypes.GetProductsSuccess: {
            return {
                ...state,
                paginator: {
                    ...state.paginator,
                    items: action.payload.items,
                    length: action.payload.length,
                },
            };
        }        
        case ProductActionTypes.SearchProduct: {
            return {
                ...state,
                searchTerm: action.payload,
                paginator: {
                    ...state.paginator,
                    pageIndex: 0,
                },
            };
        }
        case ProductActionTypes.ChangePaginator: {
            return {
                ...state,
                paginator: {
                    ...state.paginator,
                    pageIndex: action.payload.pageIndex,
                    pageSize: action.payload.pageSize,
                },
            };
        }                
        case ProductActionTypes.SubmitProductSuccess: {
            return {
                ...state,
                progress: 0,                
            };
        }                
        default:
            return state;
    }
};
