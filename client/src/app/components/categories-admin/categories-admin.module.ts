import { MaterialModule } from './../../shared/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoriesAdminRoutingModule } from './categories-admin-routing.module';
import { CategoriesAdminComponent } from './categories-admin.component';

@NgModule({
  declarations: [CategoriesAdminComponent],
  imports: [
    CommonModule,    
    FormsModule,
    ReactiveFormsModule,
    CategoriesAdminRoutingModule,        
    MaterialModule
  ]
})
export class CategoriesAdminModule { }
