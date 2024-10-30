import { HttpContextToken, HttpInterceptorFn } from '@angular/common/http';
import { LocalStorageService } from '../local-storage/local-storage.service';
import { LocalStorageKeys } from '../local-storage/local-storage-keys';
import { catchError, switchMap, throwError } from 'rxjs';
import { inject } from '@angular/core';
import { ApiService } from '../api/api.service';
import { Tokens } from '../models/api/tokens';

export const AUTH_REQUIRED = new HttpContextToken<boolean>(() => true);

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  if(!req.context.get(AUTH_REQUIRED))
    return next(req);

  const localStorage = inject(LocalStorageService);
  const api = inject(ApiService);
  const tokens = localStorage.getItem(LocalStorageKeys.tokens);
  
  if(tokens === null)
    return eraseUser(localStorage);

  const authenticatedReq = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${tokens.jwtToken}`)
  });

  return next(authenticatedReq).pipe(
    catchError((err: any) => {
      if(err.status !== 401)
        return throwError(() => err);

      return api.refreshTokens(tokens).pipe(
        switchMap((newTokens: Tokens) => {
          localStorage.setItem(LocalStorageKeys.tokens, newTokens);
          
          const reauthenticatedReq = req.clone({
            headers: req.headers.set('Authorization', `Bearer ${newTokens.jwtToken}`)
          });
          
          return next(reauthenticatedReq);
        }),
        catchError(() => eraseUser(localStorage))
      );
    })
  );
};

function eraseUser(localStorage: LocalStorageService) {
  localStorage.removeItem(LocalStorageKeys.loggedinUser);
  localStorage.removeItem(LocalStorageKeys.tokens);

  window.location.href = '/login';
  return throwError(() => new Error('Please log in'));
}