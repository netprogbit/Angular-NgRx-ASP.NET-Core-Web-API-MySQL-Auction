import { HttpHelper } from './../../shared/helpers/http.helper';
import { mergeMap } from 'rxjs/operators';
import { environment } from './../../../environments/environment';
import { Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IPaginator } from '../models/paginator.interface';
import { IProduct } from '../models/product.interface';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  
  constructor(private httpClient: HttpClient) { }

  public getProduct(id: number): Observable<any> {
    const productUrl = `${environment.apiUrl + environment.apiProduct + id}`;
    return this.httpClient.get<any>(productUrl);
  }

  public getProducts(categoryName: string, searchTerm: string, pageIndex: number, pageSize: number): Observable<IPaginator<IProduct>> {
    const productUrl = `${environment.apiUrl + environment.apiProducts}`;
    const params = HttpHelper.getPaginatorParams(categoryName, searchTerm, pageIndex, pageSize);
    return this.httpClient.get<IPaginator<IProduct>>(productUrl, { params: params }).pipe(
      mergeMap(data => {
        return of(data);
      })
    );
  }  

  public buyProduct(userId: number, productId: number): Observable<any> {
    const productBuyUrl = `${environment.apiUrl + environment.apiBuy}`;
    const body = { userId, productId };
    return this.httpClient.post(productBuyUrl, body);
  }

  public submit(id: number, categoryName: string, name: string, description: string, price: number, imageFile: File): Observable<any> {
    const formData: any = new FormData();
    formData.append("id", id);
    formData.append("categoryName", categoryName);
    formData.append("name", name);
    formData.append("description", description);
    formData.append("price", price);
    formData.append("sellerPrice", price); // Seller price becomes equal price after creating or updating    

    if (imageFile)
      formData.append("image", imageFile);

    const productSubmitUrl = `${environment.apiUrl + environment.apiProduct}`;

    if (id === 0) {
      // Addition product
      return this.httpClient.post(productSubmitUrl, formData, {
        reportProgress: true,
        observe: 'events'
      });
    }
    else {
      // Updation product
      return this.httpClient.put(productSubmitUrl, formData, {
        reportProgress: true,
        observe: 'events'
      });
    }
  }

  public deleteProduct(id: number): Observable<any> {
    const productDeleteUrl = `${environment.apiUrl + environment.apiProduct + id}`;
    return this.httpClient.delete(productDeleteUrl);
  }

}
