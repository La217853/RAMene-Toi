import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../Services/auth';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class NavbarComponent {
  constructor(
    private router: Router,
    public authService: AuthService
  ) {}

  navigateToHome(){
    this.router.navigate(['/dashboard']);
  }

  navigateToProfile() {
    this.router.navigate(['/profile']);
  }

  navigateToAdmin() {
    this.router.navigate(['/admin/users']);
  }

  navigateToAddRecette() {
    this.router.navigate(['/add-recette']);
  }

  logout() {
    this.authService.logout();
  }

  isAuthenticated(): boolean {
    return !!this.authService.getToken();
  }
}
