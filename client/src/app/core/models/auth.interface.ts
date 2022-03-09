export interface IAuth {
    userId: string,        
    token: string,
    refreshToken: string,
    roles: string[],        
}