export interface IPaginator<T> {
    items: T[],
    length: number,
    pageIndex: number,
    pageSize: number,
}