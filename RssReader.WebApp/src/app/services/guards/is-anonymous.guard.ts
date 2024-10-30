import { CanActivateFn, Router } from '@angular/router';
import { LocalStorageService } from '../local-storage/local-storage.service';
import { LocalStorageKeys } from '../local-storage/local-storage-keys';
import { inject } from '@angular/core';

export const isAnonymousGuard: CanActivateFn = (route, state) => {
  const user = inject(LocalStorageService).getItem(LocalStorageKeys.loggedinUser); 
  
  if(user === null)
    return true;
  else if(!user.isLoggedIn)
    return user.isVerified ? true : inject(Router).parseUrl('/email-verification');
  
  return false;
};
