import { Component, OnInit, Inject } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { environment } from './../../../environments/environment';
import { IAppState } from 'src/app/core/store/app/app.state';
import { progress } from 'src/app/core/store/category/category.selectors';
import { SubmitCategory } from 'src/app/core/store/category/category.actions';

@Component({
  selector: 'app-category-edit',
  templateUrl: './category-edit.component.html',
  styleUrls: ['./category-edit.component.css']
})
export class CategoryEditComponent implements OnInit {

  public progress$: Observable<number> = this.store.pipe(select(progress));
  public editForm: FormGroup;
  public title: string;
  public imageUrl: any;  

  constructor(private store: Store<IAppState>, private snackBar: MatSnackBar, private formBuilder: FormBuilder, 
    public dialogRef: MatDialogRef<CategoryEditComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {

    this.title = this.data.title;
    this.imageUrl = `${environment.apiUrl + environment.imgFolder + environment.defaultImg}`;

    this.editForm = this.formBuilder.group({
      id: ['', Validators.required],
      name: ['', Validators.required],
      image: [null]
    });

    if (this.data.id === 0) {
      this.editForm.get('id').setValue(0);
    }
    else {
      this.editForm.get('id').setValue(this.data.id);
      this.editForm.get('name').setValue(this.data.name);
      this.imageUrl = `${environment.apiUrl + environment.imgFolder + this.data.imageFileName}`;
    }    
  }

  // Editing form controls getter
  get efc() {
    return this.editForm.controls;
  }

  // Select category image file
  public select(event) {
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
    this.store.dispatch(new SubmitCategory({
      id: this.editForm.value.id,
      name: this.editForm.value.name,
      imageFileName: this.editForm.value.imageFileName,
      image: this.editForm.value.image
    }));

    this.onCancel();
  }

  public onCancel(): void {
    this.dialogRef.close();
  }  

}
