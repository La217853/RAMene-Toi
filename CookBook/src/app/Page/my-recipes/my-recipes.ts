import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecetteService } from '../../Services/recette.service';
import { AuthService } from '../../Services/auth';
import { Recette } from '../../Models/recette.model';
import { RecipeCardComponent } from '../../Shared/recipe-card/recipe-card.component';

@Component({
  selector: 'app-my-recipes',
  standalone: true,
  imports: [CommonModule, RecipeCardComponent],
  templateUrl: './my-recipes.html',
  styleUrl: './my-recipes.css'
})
export class MyRecipesComponent implements OnInit {
  private recetteService = inject(RecetteService);
  private authService = inject(AuthService);

  myRecipes = signal<Recette[]>([]);
  loading = signal<boolean>(true);
  error = signal<string | null>(null);

  ngOnInit() {
    const user = this.authService.currentUserSig();
    if (user) {
      this.loadMyRecipes(user.id);
    } else {
      this.error.set("Vous devez être connecté pour voir vos recettes.");
      this.loading.set(false);
    }
  }

  loadMyRecipes(userId: number) {
    this.recetteService.getRecettesByUtilisateur(userId).subscribe({
      next: (data) => {
        // Traitement des images
        const processedRecettes = data.map(recette => {
          const photo = recette.photo_recette?.trim();
          if (!photo) {
            recette.photo_recette = undefined;
          } else if (!photo.startsWith('http')) {
            recette.photo_recette = `assets/images/${photo}`;
          }
          return recette;
        });
        this.myRecipes.set(processedRecettes);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Erreur chargement mes recettes', err);
        // Si 404, c'est peut-être juste qu'il n'y a pas de recettes, mais l'API renvoie 404 si vide selon la doc ?
        // Vérifions le comportement. Si c'est une erreur technique, on affiche erreur.
        // Si l'API renvoie 404 quand pas de recettes, on pourrait gérer ça comme une liste vide.
        // Pour l'instant on affiche l'erreur ou liste vide si tableau vide.
        if (err.status === 404) {
             this.myRecipes.set([]);
             this.loading.set(false);
        } else {
             this.error.set("Impossible de charger vos recettes.");
             this.loading.set(false);
        }
      }
    });
  }
}
