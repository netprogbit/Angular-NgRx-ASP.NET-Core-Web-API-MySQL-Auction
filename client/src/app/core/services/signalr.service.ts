import { Injectable, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import * as signalR from "@aspnet/signalr";
import { environment } from './../../../environments/environment';
import { IAppState } from '../store/app/app.state';
import { ChangeSellerPrice } from '../store/auction/auction.actions';

// The service lives while the product-detail component lives
// This is required to reset a hub connection
@Injectable()
export class SignalrService implements OnDestroy {

  private hubConnection: signalR.HubConnection;
  private auctionHubUrl = environment.apiUrl + environment.apiAuction;
  private productId: string;   
 
  constructor(private store: Store<IAppState>) { }

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()      
      .withUrl(this.auctionHubUrl)
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Hub Connection Started.');
      })      
  }

  public addTransferStatusDataListener = (productId: number) => {
    this.productId = productId.toString();
    this.hubConnection.on(this.productId, (price) => {            
      this.store.dispatch(new ChangeSellerPrice(price));
    });
  }

  ngOnDestroy() {
    this.hubConnection.off(this.productId);
  }
}
