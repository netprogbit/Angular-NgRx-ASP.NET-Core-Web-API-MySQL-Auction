import { ProductDeleteComponent } from './components/product-delete/product-delete.component';
import { MaterialModule } from './shared/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreRouterConnectingModule } from '@ngrx/router-store';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { appReducers } from './core/store/app/app.reducer';
import { effects } from './core/store';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthLayoutComponent } from './shared/layouts/auth-layout/auth-layout.component';
import { SiteLayoutComponent } from './shared/layouts/site-layout/site-layout.component';
import { NgxPermissionsModule } from 'ngx-permissions';
import { ProductEditComponent } from './components/product-edit/product-edit.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CategoryEditComponent } from './components/category-edit/category-edit.component';
import { UserDeleteComponent } from './components/user-delete/user-delete.component';
import { UserEditComponent } from './components/user-edit/user-edit.component';
import { TokenInterceptor } from './shared/classes/token.interceptor';
import { AppErrorHandler } from './shared/classes/app-error.handler';
import { HttpErrorInterceptor } from './shared/classes/http-error.interceptor';

@NgModule({
  declarations: [
    AppComponent,    
    AuthLayoutComponent,
    SiteLayoutComponent,
    ProductEditComponent,
    ProductDeleteComponent,    
    CategoryEditComponent,
    UserDeleteComponent,
    UserEditComponent    
  ],
  entryComponents: [
    ProductEditComponent,
    ProductDeleteComponent,    
    CategoryEditComponent,
    UserDeleteComponent,
    UserEditComponent
  ],
  imports: [    
    BrowserModule,    
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    StoreModule.forRoot(appReducers),
    EffectsModule.forRoot(effects),
    StoreRouterConnectingModule.forRoot({ stateKey: 'router' }),
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    NgxPermissionsModule.forRoot()
  ],
  providers: [
    {
      provide: ErrorHandler,
      useClass: AppErrorHandler,
    },
    {
      provide: HTTP_INTERCEPTORS,
      multi: true,
      useClass: TokenInterceptor
    },
    {
      provide: HTTP_INTERCEPTORS,
      multi: true,
      useClass: HttpErrorInterceptor
    }
  ], 
  bootstrap: [AppComponent]
})
export class AppModule { }
