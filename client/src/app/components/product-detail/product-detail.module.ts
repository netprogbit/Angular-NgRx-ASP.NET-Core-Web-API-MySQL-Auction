import { MaterialModule } from './../../shared/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductDetailRoutingModule } from './product-detail-routing.module';
import { ProductDetailComponent } from './product-detail.component';

@NgModule({
  declarations: [ProductDetailComponent],
  imports: [
    CommonModule,    
    FormsModule,
    ReactiveFormsModule,
    ProductDetailRoutingModule,        
    MaterialModule
  ]  
})
export class ProductDetailModule { }
