import { Component, inject, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../Services/auth';
import { RecetteService } from '../../../Services/recette.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class ProfileComponent {

  authService = inject(AuthService);
  recetteService = inject(RecetteService);

  user = this.authService.currentUserSig;
  recettesCount = signal<number>(0);
  favoritesCount = signal<number>(0);

  constructor() {
    // Charger le nombre de recettes quand l'utilisateur est disponible
    effect(() => {
      const currentUser = this.user();
      
      if (currentUser?.id) {
        this.loadRecettesCount(currentUser.id);
        this.loadFavoriteCount(currentUser.id);
      }
    });
  }

  private loadRecettesCount(userId: number) {
    this.recetteService.getRecettesByUtilisateur(userId).subscribe({
      next: (recettes) => {
        this.recettesCount.set(recettes.length);
      },
      error: (error) => {
        // console.error('Erreur lors du chargement des recettes:', error);
        this.recettesCount.set(0);
      }
    });
  }

   private loadFavoriteCount(userId: number) {
    this.authService.getAllFavoriteRecettes(userId).subscribe({
      next: (favoris) => {
        this.favoritesCount.set(favoris.length);
      },
      error: (error) => {
      //  console.error('Erreur lors du chargement des favoris:', error);
        this.favoritesCount.set(0);
      }
    });
  }
}