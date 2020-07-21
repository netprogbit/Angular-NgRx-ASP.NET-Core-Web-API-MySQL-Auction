import { Component, OnInit, Inject } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { environment } from './../../../environments/environment';
import { ICategory } from './../../core/models/category.interface';
import { IAppState } from 'src/app/core/store/app/app.state';
import { SubmitProduct } from 'src/app/core/store/product/product.actions';
import { progress } from 'src/app/core/store/product/product.selectors';
import { allCategories } from 'src/app/core/store/category/category.selectors';
import { GetAllCategories } from 'src/app/core/store/category/category.actions';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.css']
})
export class ProductEditComponent implements OnInit {
  
  public progress$: Observable<number> = this.store.pipe(select(progress));
  public allCategories$: Observable<ICategory[]> = this.store.pipe(select(allCategories));
  public selectedCategoryName: string;
  public editForm: FormGroup;
  public title: string;  
  public imageUrl: any;        

  constructor(private store: Store<IAppState>, private snackBar: MatSnackBar, private formBuilder: FormBuilder, 
    public dialogRef: MatDialogRef<ProductEditComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {

    this.title = this.data.title;
    this.imageUrl = `${environment.apiUrl + environment.imgFolder + environment.defaultImg}`;
    this.store.dispatch(new GetAllCategories(null));    
    this.selectedCategoryName = this.data.categoryName;

    this.editForm = this.formBuilder.group({
      id: ['', Validators.required],
      categoryName: ['', Validators.required],
      name: ['', Validators.required],
      description: ['', Validators.required],
      price: ['', [Validators.required, Validators.pattern("([0-9]{1,7}[,][0-9]{2})")]],      
      image: [null]
    });

    if (this.data.id === 0) {            
      this.editForm.get('id').setValue(0);      
      this.editForm.get('price').setValue('0,00');
    }
    else {      
      this.editForm.get('id').setValue(this.data.id);      
      this.editForm.get('name').setValue(this.data.name);
      this.editForm.get('description').setValue(this.data.description);
      this.editForm.get('price').setValue(this.data.price);                  
      this.imageUrl = `${environment.apiUrl + environment.imgFolder + this.data.imageFileName}`;
    }    
  }

  // Edit form controls getter
  get efc() {
    return this.editForm.controls;
  }

  // Select product image file
  public select(event): void {
    const file = (event.target as HTMLInputElement).files[0];    
    this.editForm.patchValue({ image: file });
    this.editForm.get('image').updateValueAndValidity();
    this.editForm.patchValue({ imageFileName: file.name });

    // Image preview
    const reader = new FileReader();
    reader.onload = (event: any) => {
      this.imageUrl = event.target.result;
    };
    reader.onerror = (event: any) => {      
      this.snackBar.open("File not read. Error code: " + event.target.error.code, 'Dismiss', { duration: 2000 });
    };
    reader.readAsDataURL(file);
  }

  public onSubmit(): void {        
    this.store.dispatch(new SubmitProduct({
      id: this.editForm.value.id,
      categoryName: this.editForm.value.categoryName,
      name: this.editForm.value.name,
      description: this.editForm.value.description,
      price: this.editForm.value.price,
      image: this.editForm.value.image
    }));

    this.onCancel();
  }

  public onCancel(): void {
    this.dialogRef.close();
  }    

}
