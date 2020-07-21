import { IPaginator } from '../../models/paginator.interface';
import { ICategory } from '../../models/category.interface';

export interface ICategoryState {
    paginator: IPaginator<ICategory>,
    allCategories: ICategory[],
    selectedCategory: ICategory,    
    searchTerm: string,    
    progress: number,    
}

export const initialCategoryState: ICategoryState = {
    paginator: { items: null, length: 100, pageIndex: 0, pageSize: 10 },
    allCategories: null,
    selectedCategory: null,    
    searchTerm: null,    
    progress: 0,    
};
