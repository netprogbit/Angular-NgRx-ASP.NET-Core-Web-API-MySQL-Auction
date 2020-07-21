import { ICategory } from './category.interface';

export interface IProduct {
    id: number,
    category: ICategory,
    name: string,
    description: string,
    price: number,
    sellerPrice: number,
    imageFileName: string,
    owner: number
}