import { initialCategoryState, ICategoryState } from './category.state';
import { CategoryActions, CategoryActionTypes } from './category.actions';

export const categoryReducer = (state = initialCategoryState, action: CategoryActions): ICategoryState => {
    switch (action.type) {
        case CategoryActionTypes.GetCategoriesSuccess: {
            return {
                ...state,
                paginator: {
                    ...state.paginator,
                    items: action.payload.items,
                    length: action.payload.length,
                },
            };
        }
        case CategoryActionTypes.GetAllCategoriesSuccess: {
            return {
                ...state,
                allCategories: action.payload,
            };
        }        
        case CategoryActionTypes.SearchCategory: {
            return {
                ...state,
                searchTerm: action.payload,
                paginator: {
                    ...state.paginator,
                    pageIndex: 0,
                },
            };
        }
        case CategoryActionTypes.ChangePaginator: {
            return {
                ...state,
                paginator: {
                    ...state.paginator,
                    pageIndex: action.payload.pageIndex,
                    pageSize: action.payload.pageSize,
                },
            };
        }
        case CategoryActionTypes.SelectCategory: {
            return {
                ...state,
                selectedCategory: action.payload,
            };
        }
        case CategoryActionTypes.SubmitCategorySuccess: {
            return {
                ...state,                
                progress: 0,                
            };
        }        
        default:
            return state;
    }
};