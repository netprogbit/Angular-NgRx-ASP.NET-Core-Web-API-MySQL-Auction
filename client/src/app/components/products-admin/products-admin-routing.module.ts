import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductsAdminComponent } from './products-admin.component';

const routes: Routes = [
  {
    path: '',
    component: ProductsAdminComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]  
})
export class ProductsAdminRoutingModule { }
