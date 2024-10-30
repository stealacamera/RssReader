import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ControlErrorComponent } from '../../services/validation/controll-error.component';
import { ApiService } from '../../services/api/api.service';
import { Router } from '@angular/router';
import { showErrors } from '../../services/utils/show-errors';
import { ToastrService } from 'ngx-toastr';
import { LocalStorageService } from '../../services/local-storage/local-storage.service';
import { LocalUser } from '../../services/models/local-user';
import { LocalStorageKeys } from '../../services/local-storage/local-storage-keys';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, ControlErrorComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  constructor(
    private router: Router,
    private api: ApiService,
    private localStorage: LocalStorageService,
    private toastr: ToastrService) {}

  errors : string[] = [];

  form = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', Validators.required)
  });

  login() {
    this.api.login(this.form.value.email!, this.form.value.password!)
      .subscribe({
        next: loggedUser => {
          const currUser: LocalUser = { isLoggedIn: true, isVerified: true, user: loggedUser.user };
          
          this.localStorage.setItem(LocalStorageKeys.loggedinUser, currUser);
          this.localStorage.setItem(LocalStorageKeys.tokens, loggedUser.tokens);
          
          this.router.navigateByUrl('/dashboard');
        },
        error: err => {
          if(err.status === 403)
            this.router.navigateByUrl('/email-verification');
          else
            showErrors(err, this.errors, this.toastr)
        }
      });
  }
}
