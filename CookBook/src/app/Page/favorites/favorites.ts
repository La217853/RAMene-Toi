import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecetteService } from '../../Services/recette.service';
import { AuthService } from '../../Services/auth';
import { Recette } from '../../Models/recette.model';
import { RecipeCardComponent } from '../../Shared/recipe-card/recipe-card.component';

@Component({
  selector: 'app-favorites',
  standalone: true,
  imports: [CommonModule, RecipeCardComponent],
  templateUrl: './favorites.html',
  styleUrl: './favorites.css'
})
export class FavoritesComponent implements OnInit {
  private recetteService = inject(RecetteService);
  private authService = inject(AuthService);

  favorites = signal<Recette[]>([]);
  loading = signal<boolean>(true);
  error = signal<string | null>(null);

  ngOnInit() {
    const user = this.authService.currentUserSig();
    if (user) {
      this.loadFavorites(user.id);
    } else {
      this.error.set("Vous devez être connecté pour voir vos favoris.");
      this.loading.set(false);
    }
  }

  loadFavorites(userId: number) {
    this.recetteService.getFavoris(userId).subscribe({
      next: (data) => {
        // Traitement des images comme dans details-recette
        const processedRecettes = data.map(recette => {
          const photo = recette.photo_recette?.trim();
          if (!photo) {
            recette.photo_recette = undefined;
          } else if (!photo.startsWith('http')) {
            recette.photo_recette = `assets/images/${photo}`;
          }
          return recette;
        });
        this.favorites.set(processedRecettes);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Erreur chargement favoris', err);
        this.error.set("Impossible de charger vos favoris.");
        this.loading.set(false);
      }
    });
  }
}
