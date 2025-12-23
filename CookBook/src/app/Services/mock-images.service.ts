import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MockImagesService {

  // Collection d'images de recettes gratuites
  private recetteImages = [
   //apre ca
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
