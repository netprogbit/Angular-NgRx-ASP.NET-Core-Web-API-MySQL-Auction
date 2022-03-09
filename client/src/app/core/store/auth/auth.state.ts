import { IAuth } from '../../models/auth.interface';

export interface IAuthState {
    authData: IAuth,  
}

export const initialAuthState: IAuthState = {
    authData: { userId: '', token: '', refreshToken: '', roles: [] },
};