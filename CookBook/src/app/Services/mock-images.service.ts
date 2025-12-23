import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MockImagesService {

  // Collection d'images de recettes gratuites
  private recetteImages = [
    'https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=400&h=300&fit=crop', // Salade
    'https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=400&h=300&fit=crop', // Pizza
    'https://images.unsplash.com/photo-1567620905732-2d1ec7ab7445?w=400&h=300&fit=crop', // Pancakes
    'https://images.unsplash.com/photo-1540189549336-e6e99c3679fe?w=400&h=300&fit=crop', // Bowl
    'https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=400&h=300&fit=crop', // Burger
    'https://images.unsplash.com/photo-1565958011703-44f9829ba187?w=400&h=300&fit=crop', // Sushi
    'https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=400&h=300&fit=crop', // Steak
    'https://images.unsplash.com/photo-1563379926898-05f4575a45d8?w=400&h=300&fit=crop', // Pâtes
    'https://images.unsplash.com/photo-1551782450-a2132b4ba21d?w=400&h=300&fit=crop', // Burger veggie
    'https://images.unsplash.com/photo-1547592180-85f173990554?w=400&h=300&fit=crop', // Sandwich
    'https://images.unsplash.com/photo-1512621776951-a57141f2eefd?w=400&h=300&fit=crop', // Salade verte
    'https://images.unsplash.com/photo-1623428187969-5da2dcea5ebf?w=400&h=300&fit=crop', // Petit déj
  ];

  /**
   * Récupère une image aléatoire
   */
  getRandomImage(): string {
    const randomIndex = Math.floor(Math.random() * this.recetteImages.length);
    return this.recetteImages[randomIndex];
  }

  /**
   * Récupère une image spécifique par index
   */
  getImageByIndex(index: number): string {
    const safeIndex = index % this.recetteImages.length;
    return this.recetteImages[safeIndex];
  }

  /**
   * Récupère toutes les images disponibles
   */
  getAllImages(): string[] {
    return [...this.recetteImages];
  }

  /**
   * Génère une image de placeholder avec texte personnalisé
   */
  getPlaceholderImage(text: string = 'Recette', width: number = 400, height: number = 300): string {
    return `https://via.placeholder.com/${width}x${height}/FF6B6B/FFFFFF?text=${encodeURIComponent(text)}`;
  }

  /**
   * Récupère une image de placeholder colorée
   */
  getColoredPlaceholder(color: string = 'FF6B6B', width: number = 400, height: number = 300): string {
    return `https://via.placeholder.com/${width}x${height}/${color}/FFFFFF?text=Recette`;
  }
}
