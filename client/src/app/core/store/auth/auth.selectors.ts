import { IAppState } from '../app/app.state';
import { createSelector } from '@ngrx/store';
import { IAuthState } from './auth.state';

const auth = (state: IAppState) => state.auth;
export const authData = createSelector(auth, (state: IAuthState) => state.authData);
