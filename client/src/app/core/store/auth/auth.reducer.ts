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
                    role: action.payload.role,
                    token: action.payload.token,
                },
            };
        }                 
        default:
            return state;
    }
};
