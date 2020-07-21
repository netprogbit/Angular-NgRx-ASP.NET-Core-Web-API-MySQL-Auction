import { SiteLayoutComponent } from './shared/layouts/site-layout/site-layout.component';
import { AuthLayoutComponent } from './shared/layouts/auth-layout/auth-layout.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { AdminGuard } from './core/guards/admin.guards';

const routes: Routes = [
  {
    path: '',
    component: SiteLayoutComponent,
    canActivate: [AuthGuard],    
    data: { pageTitle: "Site"},
    children: [
      {
        path: '', redirectTo: '/products', pathMatch: 'full'
      },
      { 
        path: 'products', 
        loadChildren: './components/product-list/product-list.module#ProductListModule'
      },
      { 
        path: 'products/:id', 
        loadChildren: './components/product-detail/product-detail.module#ProductDetailModule'
      },
      { 
        path: 'users-admin',
        canActivate: [AdminGuard], 
        loadChildren: './components/users-admin/users-admin.module#UsersAdminModule'
      },
      { 
        path: 'categories-admin',
        canActivate: [AdminGuard], 
        loadChildren: './components/categories-admin/categories-admin.module#CategoriesAdminModule'
      },
      { 
        path: 'products-admin',
        canActivate: [AdminGuard],                
        loadChildren: './components/products-admin/products-admin.module#ProductsAdminModule'
      }      
    ]
  }, 
  {
    path: '',
    component: AuthLayoutComponent,
    data: { pageTitle: "Auth"},
    children: [
      {
        path: '', redirectTo: '/login', pathMatch: 'full'
      },
      { 
        path: 'register', 
        loadChildren: './components/register/register.module#RegisterModule'
      },
      {
        path: 'login',
        loadChildren: './components/login/login.module#LoginModule'
      }
    ]
  },
     
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
