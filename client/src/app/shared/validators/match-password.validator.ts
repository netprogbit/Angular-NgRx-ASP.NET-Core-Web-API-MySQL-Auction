import { FormGroup } from '@angular/forms';

export function MustMatch(password: string, confirmPassword: string) {
    return (formGroup: FormGroup) => {
        const control = formGroup.controls[password];
        const matchingControl = formGroup.controls[confirmPassword];

        if (matchingControl.errors && !matchingControl.errors.mustMatch) {            
            return;
        }
    
        if (control.value !== matchingControl.value) {
            matchingControl.setErrors({ mustMatch: true });
        } else {
            matchingControl.setErrors(null);
        }
    }
}