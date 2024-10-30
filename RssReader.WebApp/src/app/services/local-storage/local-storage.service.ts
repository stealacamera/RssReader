import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { StorageKey } from './local-storage-keys';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {
  private storage: Storage | null;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    this.storage = isPlatformBrowser(this.platformId) ? localStorage : null;
  }

  setItem<T>(item: StorageKey<T>, value: T) : void {
    if(this.storage)
      localStorage.setItem(item.key, JSON.stringify(value));
  }

  getItem<T>(item: StorageKey<T>): T | null { 
    if(!this.storage)
      return null;
    
    const value = localStorage.getItem(item.key);
    return value === null ? null : JSON.parse(value) as typeof item.value;
  }

  removeItem<T>(item: StorageKey<T>): void {
    if(this.storage)
      localStorage.removeItem(item.key);
  }

  clear(): void {
    if(this.storage)
      localStorage.clear();
  }
}
