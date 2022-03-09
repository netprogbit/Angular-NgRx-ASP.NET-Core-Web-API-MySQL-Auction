import { initialAuthState, IAuthState } from './auth.state';
import { AuthActions, AuthActionTypes } from './auth.actions';

export const authReducer = (state = initialAuthState, action: AuthActions): IAuthState => {
    switch (action.type) {                
        case AuthActionTypes.LoginSuccess: {
            return {
                ...state,               
                authData: {
                    ...state.authData,
                    userId: action.payload.userId,
                    token: action.payload.token,
                    refreshToken: action.payload.refreshToken,
                    roles: action.payload.roles,                                        
                },
            };
        }
        case AuthActionTypes.LogoutSuccess: {
            return {
                ...state,               
                authData: {
                    ...state.authData,
                    userId: '',
                    token: '',
                    refreshToken: '',
                    roles: [],                                        
                },
            };
        }
        case AuthActionTypes.SetAuth: {
            return {
                ...state,               
                authData: {
                    ...state.authData,
                    userId: action.payload.userId,
                    token: action.payload.token,
                    refreshToken: action.payload.refreshToken,
                    roles: action.payload.roles,                                        
                },
            };
        }
        case AuthActionTypes.SetToken: {
            return {
                ...state,               
                authData: {
                    ...state.authData,
                    token: action.payload.token,
                },
            };
        }                 
        default:
            return state;
    }
};
