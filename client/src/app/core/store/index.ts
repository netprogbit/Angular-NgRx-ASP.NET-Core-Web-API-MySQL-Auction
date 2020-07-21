import { AuthEffects } from './auth/auth.effects';
import { AuctionEffects } from './auction/auction.effects';
import { ProductEffects } from './product/product.effects';
import { CategoryEffects } from './category/category.effects';
import { UserEffects } from './user/user.effects';

export const effects = [
    AuthEffects,
    AuctionEffects,
    ProductEffects,
    CategoryEffects,
    UserEffects,    
];
