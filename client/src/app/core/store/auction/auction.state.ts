import { IPaginator } from '../../models/paginator.interface';
import { IProduct } from '../../models/product.interface';

export interface IAuctionState {
  paginator: IPaginator<IProduct>,
  selectedProduct: IProduct;
  categoryName: string,
  searchTerm: string,
  sellerPrice: number,    
}

export const initialAuctionState: IAuctionState = {
  paginator: { items: null, length: 100, pageIndex: 0, pageSize: 10 },
  selectedProduct: null,
  categoryName: null,
  searchTerm: null,
  sellerPrice: 0,    
};
