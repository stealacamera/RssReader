import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApiService } from '../../services/api/api.service';
import { LocalStorageKeys } from '../../services/local-storage/local-storage-keys';
import { LocalStorageService } from '../../services/local-storage/local-storage.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-email-verification',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './email-verification.component.html',
  styleUrl: './email-verification.component.css'
})
export class EmailVerificationComponent {
  constructor(
    private readonly router: Router,
    private readonly api: ApiService,
    private readonly localStorage: LocalStorageService,
    private readonly toastr: ToastrService) 
  {}
  
  verificationForm = new FormGroup({
    otp: new FormControl('', Validators.required)
  });
    
  submitVerification() {
    const user = this.localStorage.getItem(LocalStorageKeys.loggedinUser);

    if(user === null) {
      this.toastr.error('Redirecting to login page. If problem persists, contact us.', 'Something went wrong :(');
      setTimeout(() => this.router.navigateByUrl('/login'), 5000);
    } 
    else {
      this.api.verifyEmail(user.user.id, this.verificationForm.value.otp!)
        .subscribe({
          next: () => {
            user.isVerified = true;
            this.localStorage.setItem(LocalStorageKeys.loggedinUser, user);

            this.toastr.success('You will be redirected to the login page in a few seconds', 'Verification successful!');
            setTimeout(() => this.router.navigateByUrl('/login'), 4000);
          },
          error: () => this.toastr.error('Make sure the password is written correctly and/or not expired')
        });
    }
  }

  resendVerificationPassword() {
    const user = (this.localStorage.getItem(LocalStorageKeys.loggedinUser))!;

    this.api.resendVerificationPassword(user.user.id)
      .subscribe({
        next: () => this.toastr.success('New OTP sent successfully'),
        error: () => this.toastr.error('Try again in a moment. If the problem persists, contact us.', 'Something went wront :('),
      });
  }
}
