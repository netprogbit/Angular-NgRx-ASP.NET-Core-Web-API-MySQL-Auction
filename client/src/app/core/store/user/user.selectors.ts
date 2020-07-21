import { IAppState } from '../app/app.state';
import { IUserState } from './user.state';
import { createSelector } from '@ngrx/store';

const user = (state: IAppState) => state.user;
export const paginator = createSelector(user, (state: IUserState) => state.paginator);
export const searchTerm = createSelector(user, (state: IUserState) => state.searchTerm);
export const uploaded = createSelector(user, (state: IUserState) => state.uploaded);