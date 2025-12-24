import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../Services/auth';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.css']
})
export class NavbarComponent {
  constructor(
    private router: Router,
    public authService: AuthService
  ) {}

  navigateToProfile() {
    this.router.navigate(['/profile']);
  }

  navigateToAdmin() {
    this.router.navigate(['/admin/users']);
  }

  navigateToAddRecette() {
    this.router.navigate(['/add-recette']);
  }

  navigateToFavorites() {
    this.router.navigate(['/favoris']);
  }

  navigateToMyRecipes() {
    this.router.navigate(['/mes-recettes']);
  }

  logout() {
    this.authService.logout();
  }

  isAuthenticated(): boolean {
    return !!this.authService.getToken();
  }

  navigateToDecouvrir(){
    this.router.navigate(['/decouvrir'])
  }
}
