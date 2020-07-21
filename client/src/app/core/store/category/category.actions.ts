import { Action } from '@ngrx/store';
import { IPaginator } from '../../models/paginator.interface';
import { ICategory } from '../../models/category.interface';

export enum CategoryActionTypes {
    GetCategories = '[Category] Get Categories',
    GetCategoriesSuccess = '[Category] Get Categories Success',
    GetAllCategories = '[Category] Get All Categories',
    GetAllCategoriesSuccess = '[Category] Get All Categories Success',    
    ChangePaginator = '[Category] Change Paginator',
    SearchCategory = '[Category] Search Category',
    SubmitCategory = '[Category] Submit Category',
    SubmitCategorySuccess = '[Category] Submit Category Success',    
    ChangeProgress = '[Category] Change Progress',
    SelectCategory = '[Category] Select Category',
    DeleteCategory = '[Category] Delete Category',
}

export class GetCategories implements Action {
    public readonly type = CategoryActionTypes.GetCategories;    
}

export class GetCategoriesSuccess implements Action {
    public readonly type = CategoryActionTypes.GetCategoriesSuccess;
    constructor(public payload: IPaginator<ICategory>) { }
}

export class GetAllCategories implements Action {
    public readonly type = CategoryActionTypes.GetAllCategories;
    constructor(public payload: ICategory) { }    
}

export class GetAllCategoriesSuccess implements Action {
    public readonly type = CategoryActionTypes.GetAllCategoriesSuccess;
    constructor(public payload: ICategory[]) { }
}

export class ChangePaginator implements Action {
    public readonly type = CategoryActionTypes.ChangePaginator;
    constructor(public payload: any) { }
}

export class SearchCategory implements Action {
    public readonly type = CategoryActionTypes.SearchCategory;
    constructor(public payload: string) { }
}

export class SubmitCategory implements Action {
    public readonly type = CategoryActionTypes.SubmitCategory;
    constructor(public payload: any) { }
}

export class SubmitCategorySuccess implements Action {
    public readonly type = CategoryActionTypes.SubmitCategorySuccess;
    constructor(public payload: string) { }    
}

export class ChangeProgress implements Action {
    public readonly type = CategoryActionTypes.ChangeProgress;
    constructor(public payload: number) { }
}

export class SelectCategory implements Action {
    public readonly type = CategoryActionTypes.SelectCategory;
    constructor(public payload: ICategory) { }
}

export class DeleteCategory implements Action {
    public readonly type = CategoryActionTypes.DeleteCategory;
    constructor(public payload: number) { }
}

export type CategoryActions =
    | GetCategories
    | GetCategoriesSuccess
    | GetAllCategories
    | GetAllCategoriesSuccess
    | ChangePaginator
    | SearchCategory
    | SubmitCategory
    | SubmitCategorySuccess    
    | ChangeProgress
    | SelectCategory
    | DeleteCategory;