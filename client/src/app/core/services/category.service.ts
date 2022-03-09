import { HttpHelper } from './../../shared/helpers/http.helper';
import { ICategory } from './../models/category.interface';
import { mergeMap, map } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../../environments/environment';
import { Injectable  } from '@angular/core';
import { IPaginator } from '../models/paginator.interface';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  
  constructor(private httpClient: HttpClient) { }

  public getAllCategories(): Observable<ICategory[]> {    
    const categoryUrl = `${environment.apiUrl + environment.apiAllCategories}`;            
    return this.httpClient.get<any>(categoryUrl).pipe(
      map(categories => {        
        return categories.map(category => {
          return {id: category.id, name: category.name, imageFileName: category.imageFileName };
        });
      })
    );
  }

  public getCategories(searchTerm: string, pageIndex: number, pageSize: number): Observable<IPaginator<ICategory>> {
    const categoryUrl = `${environment.apiUrl + environment.apiCategories}`;    
    const params = HttpHelper.getPaginatorParams(null, searchTerm, pageIndex, pageSize);    
    return this.httpClient.get<IPaginator<ICategory>>(categoryUrl, { params: params }).pipe(
      mergeMap(data => {        
        return of(data);
      })
    );
  }
  
  public submit(id: number, name: string, imageFileName: string, imageFile: File): Observable<any> {
    const formData: any = new FormData();
    formData.append("id", id);    
    formData.append("name", name);    
    formData.append("imageFileName", imageFileName);

    if (imageFile)
      formData.append("image", imageFile);

    const categorySubmitUrl = `${environment.apiUrl + environment.apiCategory}`;    

    if (id === 0) {
      // Addition category
      return this.httpClient.post(categorySubmitUrl, formData, {        
        reportProgress: true,
        observe: 'events'
      });
    }
    else {
      // Updation category
      return this.httpClient.put(categorySubmitUrl, formData, {        
        reportProgress: true,
        observe: 'events'
      });
    }    
  }  

}
