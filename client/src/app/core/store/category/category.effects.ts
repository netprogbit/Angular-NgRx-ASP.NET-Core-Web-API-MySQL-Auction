import { Effect, ofType, Actions } from '@ngrx/effects';
import { select, Store } from '@ngrx/store';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { MatSnackBar } from '@angular/material';
import {
    GetCategories,
    GetCategoriesSuccess,
    SearchCategory,
    GetAllCategories,
    GetAllCategoriesSuccess,
    SubmitCategory,
    SubmitCategorySuccess,
    CategoryActionTypes,
    ChangeProgress
} from './category.actions';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { switchMap, map, withLatestFrom, debounceTime, distinctUntilChanged, concatMap, takeUntil, tap } from 'rxjs/operators';
import { paginator, searchTerm } from './category.selectors';
import { IAppState } from '../app/app.state';
import { IPaginator } from '../../models/paginator.interface';
import { ICategory } from '../../models/category.interface';
import { CategoryService } from '../../services/category.service';

@Injectable()
export class CategoryEffects {

    @Effect()
    getCategories$ = this.actions$.pipe(
        ofType<GetCategories>(CategoryActionTypes.GetCategories),
        withLatestFrom(
            this.store.pipe(select(searchTerm)),
            this.store.pipe(select(paginator))
        ),
        switchMap(([payload, searchTerm, paginator]) => {
            return this.categoryService.getCategories(searchTerm, paginator.pageIndex, paginator.pageSize);
        }),
        switchMap((paginator: IPaginator<ICategory>) => {
            return of(new GetCategoriesSuccess(paginator));
        })
    );

    @Effect()
    searchCategory$ = this.actions$.pipe(
        ofType<SearchCategory>(CategoryActionTypes.SearchCategory),
        debounceTime(300),
        distinctUntilChanged(),
        map(action => action.payload),
        withLatestFrom(
            this.store.pipe(select(paginator))
        ),
        switchMap(([payload, paginator]) => {
            return this.categoryService.getCategories(payload, paginator.pageIndex, paginator.pageSize);
        }),
        switchMap((paginator: IPaginator<ICategory>) => {
            return of(new GetCategoriesSuccess(paginator));
        })
    );

    @Effect()
    getAllCategories$ = this.actions$.pipe(
        ofType<GetAllCategories>(CategoryActionTypes.GetAllCategories),
        map(action => action.payload),
        switchMap((payload) => {
            return this.categoryService.getAllCategories().pipe(
                map(data => {
                    
                    if (payload) {
                        return [payload].concat(data);
                    }

                    return data;
                })
            );
        }),
        switchMap((categories: ICategory[]) => {
            return of(new GetAllCategoriesSuccess(categories));
        })
    );

    @Effect()
    submitCategory$ = this.actions$.pipe(
        ofType<SubmitCategory>(CategoryActionTypes.SubmitCategory),
        map(action => action.payload),
        switchMap(payload => {
            return this.categoryService.submit(payload.id, payload.name, payload.imageFileName, payload.image);
        }),
        switchMap((event: HttpEvent<any>) => {
            switch (event.type) {
                case HttpEventType.UploadProgress:
                    return of(new ChangeProgress(Math.round(event.loaded / event.total * 100)));
                case HttpEventType.Response:
                    return [new SubmitCategorySuccess(event.body.message), new GetCategories()];
                default:
                    return of();
            }
        })
    );

    @Effect({ dispatch: false })
    submitCategorySuccess$ = this.actions$.pipe(
        ofType<SubmitCategorySuccess>(CategoryActionTypes.SubmitCategorySuccess),
        map(action => action.payload),        
        tap(payload => {
            this.snackBar.open(payload, 'OK', { duration: 3000 });
        }),
    );

    constructor(
        private categoryService: CategoryService,
        private actions$: Actions,
        private store: Store<IAppState>,
        private snackBar: MatSnackBar,
    ) { }
}
