import { MaterialModule } from './../../shared/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersAdminRoutingModule } from './users-admin-routing.module';
import { UsersAdminComponent } from './users-admin.component';

@NgModule({
  declarations: [UsersAdminComponent],
  imports: [
    CommonModule,    
    FormsModule,
    ReactiveFormsModule,
    UsersAdminRoutingModule,     
    MaterialModule
  ]
})
export class UsersAdminModule { }
