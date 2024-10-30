import { CanActivateFn, Router } from '@angular/router';
import { LocalStorageService } from '../local-storage/local-storage.service';
import { LocalStorageKeys } from '../local-storage/local-storage-keys';
import { inject } from '@angular/core';

export const isSignedUpGuard: CanActivateFn = (route, state) => {
  const user = inject(LocalStorageService).getItem(LocalStorageKeys.loggedinUser);

  if(user !== null && !user.isVerified)
    return true;

  return inject(Router).parseUrl('/email-verification');
};
