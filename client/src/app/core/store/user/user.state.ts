import { IPaginator } from '../../models/paginator.interface';
import { IUser } from '../../models/user.interface';

export interface IUserState {
    paginator: IPaginator<IUser>,    
    searchTerm: string,
    uploaded: boolean,        
}

export const initialUserState: IUserState = {
    paginator: { items: null, length: 100, pageIndex: 0, pageSize: 10 },    
    searchTerm: null,
    uploaded: false,        
};
