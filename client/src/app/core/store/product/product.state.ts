import { IPaginator } from '../../models/paginator.interface';
import { IProduct } from '../../models/product.interface';

export interface IProductState {
  paginator: IPaginator<IProduct>,    
  searchTerm: string,  
  progress: number,  
}

export const initialProductState: IProductState = {
  paginator: { items: null, length: 100, pageIndex: 0, pageSize: 10 },    
  searchTerm: null,  
  progress: 0,  
};
