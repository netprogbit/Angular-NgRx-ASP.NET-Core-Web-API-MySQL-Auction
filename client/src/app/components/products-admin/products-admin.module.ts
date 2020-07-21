import { MaterialModule } from './../../shared/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductsAdminRoutingModule } from './products-admin-routing.module';
import { ProductsAdminComponent } from './products-admin.component';

@NgModule({
  declarations: [ProductsAdminComponent],
  imports: [
    CommonModule,    
    FormsModule,
    ReactiveFormsModule,
    ProductsAdminRoutingModule,        
    MaterialModule    
  ]
})
export class ProductsAdminModule { }
