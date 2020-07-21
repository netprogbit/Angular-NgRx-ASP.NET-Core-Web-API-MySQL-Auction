import { initialUserState, IUserState } from './user.state';
import { UserActions, UserActionTypes } from './user.actions';

export const userReducer = (state = initialUserState, action: UserActions): IUserState => {
    switch (action.type) {
        case UserActionTypes.GetUsersSuccess: {
            return {
                ...state,
                paginator: {
                    ...state.paginator,
                    items: action.payload.items,
                    length: action.payload.length,
                },
            };
        }        
        case UserActionTypes.SearchUser: {
            return {
                ...state,
                searchTerm: action.payload,
                paginator: {
                    ...state.paginator,
                    pageIndex: 0,
                },
            };
        }
        case UserActionTypes.ChangePaginator: {
            return {
                ...state,
                paginator: {
                    ...state.paginator,
                    pageIndex: action.payload.pageIndex,
                    pageSize: action.payload.pageSize,
                },
            };
        }                
        default:
            return state;
    }
};