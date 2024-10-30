import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { LocalStorageService } from '../local-storage/local-storage.service';
import { LocalStorageKeys } from '../local-storage/local-storage-keys';

export const isLoggedInGuard: CanActivateFn = (route, state) => {
  const user = inject(LocalStorageService).getItem(LocalStorageKeys.loggedinUser);

  if(user !== null && user.isLoggedIn && user.isVerified )
    return true;

  return inject(Router).parseUrl('/login');
};
