import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApiService } from '../../services/api/api.service';
import { ValidationUtils } from '../../services/validation/validation-utils';
import { ControlErrorComponent } from '../../services/validation/controll-error.component';
import { NgIf } from '@angular/common';
import { LocalStorageService } from '../../services/local-storage/local-storage.service';
import { LocalStorageKeys } from '../../services/local-storage/local-storage-keys';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';
import { error } from 'console';
import { showErrors } from '../../services/utils/show-errors';
import { LocalUser } from '../../services/models/local-user';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [ReactiveFormsModule, ControlErrorComponent, NgIf],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  constructor(
    private router: Router,
    private api: ApiService,
    private localStorage: LocalStorageService,
    private toastr: ToastrService) {}

  form = new FormGroup({
    username: new FormControl<string>('', Validators.maxLength(100)),
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    password: new FormControl<string>(
      '', 
      [Validators.required, 
        Validators.minLength(8), 
        Validators.maxLength(30),
        Validators.pattern(ValidationUtils.passwordPattern)])
  });

  responseErrors: string[] = [];

  submitForm() {
    if(this.form.invalid)
      this.form.markAllAsTouched();

    this.api.signup(this.form.value.email!, this.form.value.password!, this.form.value.username!)
      .subscribe({
        next: createdUser => {
          const currentUser: LocalUser = {user: createdUser, isLoggedIn: false, isVerified: false};

          this.localStorage.setItem(LocalStorageKeys.loggedinUser, currentUser);
          this.router.navigateByUrl('/email-verification');
        },
        error: err => showErrors(err, this.responseErrors, this.toastr)
      });
  }
}
