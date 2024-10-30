import { Component, DoCheck } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { LocalStorageService } from '../../services/local-storage/local-storage.service';
import { LocalStorageKeys } from '../../services/local-storage/local-storage-keys';
import { LocalUser } from '../../services/models/local-user';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements DoCheck {
  currentUser: LocalUser | null = null;

  constructor(
    private localStorage: LocalStorageService,
    private router: Router
  ) 
  {}

  ngDoCheck(): void {
    this.currentUser = this.localStorage.getItem(LocalStorageKeys.loggedinUser) || null;
  }

  logout() {
    this.localStorage.removeItem(LocalStorageKeys.loggedinUser);
    this.localStorage.removeItem(LocalStorageKeys.tokens);

    this.router.navigateByUrl('/login');
  }
}
