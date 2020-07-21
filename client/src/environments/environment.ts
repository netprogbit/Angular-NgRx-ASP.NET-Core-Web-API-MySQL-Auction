// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/',
  apiAuction: 'auction/',  
  apiRegister: 'api/auth/register/',
  apiLogin: 'api/auth/login/',
  apiUser: 'api/user/',
  apiUsers: 'api/user/users/',
  apiProduct: 'api/product/',
  apiProducts: 'api/product/products/',
  apiBuy: 'api/product/buy/',
  apiCategory: 'api/category/',
  apiAllCategories: 'api/category/allcategories/',
  apiCategories: 'api/category/categories/',
  apiErrorLog: 'api/error/log',
  imgFolder: 'images/',
  defaultImg: 'default.png'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
