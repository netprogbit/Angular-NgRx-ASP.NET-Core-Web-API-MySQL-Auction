import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { AuthService } from 'src/app/core/services/auth.service';
import { SignalrService } from './../../core/services/signalr.service';
import { IProduct } from './../../core/models/product.interface';
import { Observable } from 'rxjs';
import { environment } from './../../../environments/environment';
import { IAppState } from 'src/app/core/store/app/app.state';
import { selectedProduct, sellerPrice } from 'src/app/core/store/auction/auction.selectors';
import { GetProduct, BuyProduct } from 'src/app/core/store/auction/auction.actions';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css'],
  providers: [SignalrService]
})
export class ProductDetailComponent implements OnInit {

  public product$: Observable<IProduct> = this.store.pipe(select(selectedProduct));
  public sellerPrice$: Observable<number> = this.store.pipe(select(sellerPrice));  
  public imageUrl: any = `${environment.apiUrl + environment.imgFolder}`;      

  constructor(private store: Store<IAppState>, private route: ActivatedRoute, 
    private authService: AuthService, private signalRService: SignalrService) { }

  ngOnInit() {
        
    // Getting product by id
    const id = +this.route.snapshot.paramMap.get('id');
    this.store.dispatch(new GetProduct(id));
         
    this.signalRService.startConnection();
    this.signalRService.addTransferStatusDataListener(id);
  }

  // Bid offer
  public buy(productId: number): void {
    const userId: string = this.authService.getUserId();
    this.store.dispatch(new BuyProduct({ userId: userId, productId: productId}));
  }  

}
