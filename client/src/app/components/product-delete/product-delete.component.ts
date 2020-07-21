import { Component, OnInit, Inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { environment } from 'src/environments/environment';
import { IAppState } from 'src/app/core/store/app/app.state';
import { DeleteProduct } from 'src/app/core/store/product/product.actions';

@Component({
  selector: 'app-product-delete',
  templateUrl: './product-delete.component.html',
  styleUrls: ['./product-delete.component.css']
})
export class ProductDeleteComponent implements OnInit {

  public title: string;
  public imageUrl: any;    

  constructor(private store: Store<IAppState>, public dialogRef: MatDialogRef<ProductDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
    this.title = this.data.title;
    this.imageUrl = `${environment.apiUrl + environment.imgFolder + this.data.imageFileName}`;    
  }

  onDelete(): void {
    this.store.dispatch(new DeleteProduct(this.data.id));
    this.onCancel();
  }

  onCancel(): void {
    this.dialogRef.close();
  }  

}
