import { IAppState } from '../app/app.state';
import { ICategoryState } from './category.state';
import { createSelector } from '@ngrx/store';

const category = (state: IAppState) => state.category;
export const paginator = createSelector(category, (state: ICategoryState) => state.paginator);
export const allCategories = createSelector(category, (state: ICategoryState) => state.allCategories);
export const selectedCategory = createSelector(category, (state: ICategoryState) => state.selectedCategory);
export const searchTerm = createSelector(category, (state: ICategoryState) => state.searchTerm);
export const progress = createSelector(category, (state: ICategoryState) => state.progress);