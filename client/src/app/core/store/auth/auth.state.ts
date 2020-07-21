import { IAuth } from '../../models/auth.interface';

export interface IAuthState {
    authData: IAuth,  
}

export const initialAuthState: IAuthState = {
    authData: { userId: 0, role: null, token: null }, 
};