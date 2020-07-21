import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { ErrorService } from 'src/app/core/services/error.service';
import { StringHelper } from '../helpers/string.helper';

@Injectable()
export class AppErrorHandler implements ErrorHandler {
  
  constructor(private injector: Injector) { }

  get router(): Router {
    return this.injector.get(Router);
  };

  get snackBar(): MatSnackBar {
    return this.injector.get(MatSnackBar);
  };

  get errorService(): ErrorService {
    return this.injector.get(ErrorService);
  };

  // Called on an unhandled errors
  handleError(error) {

    // Application error handling
    if (error instanceof Error || error instanceof TypeError) {
      this.errorService.log(error.stack).subscribe();
      this.snackBar.open(`Error. ${StringHelper.IT_WILL_BE_FIXED}`, 'Dismiss', { duration: 3000 });      
    }         
  }
}