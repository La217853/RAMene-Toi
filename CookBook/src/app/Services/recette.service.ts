import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Recette,Etape,Categorie } from '../Models/recette.model';
import { User } from '../Models/user.model';
import { Ingredient, IngredientQuantite } from '../Models/ingredient.models';

@Injectable({
  providedIn: 'root'
})
export class RecetteService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5230/api/Recettes';
  private baseUrl = 'http://localhost:5230/api';

  /**
   * Récupère toutes les recettes d'un utilisateur
   */
  getRecettesByUtilisateur(utilisateurId: number): Observable<Recette[]> {
    return this.http.get<Recette[]>(`${this.apiUrl}/Utilisateur/${utilisateurId}`);
  }
  
  getEtapes(id: number): Observable<Etape[]> { 
    return this.http.get<Etape[]>(`${this.baseUrl}/Etapes/${id}`);
  }
  
  getAuteur(id: number): Observable<User> { 
    return this.http.get<User>(`${this.baseUrl}/Utilisateurs/${id}`); 
  }

  getIngredient(id: number) { 
    return this.http.get<Ingredient>(`${this.baseUrl}/Ingredients/${id}`); 
  }

  /**
   * Upload une image pour une recette
   */
  uploadRecetteImage(recetteId: number, file: File): Observable<{ imageUrl: string }> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<{ imageUrl: string }>(
      `${this.apiUrl}/${recetteId}/upload-image`,
      formData
    );
  }

  /**
   * Récupère toutes les recettes
   */
  getAllRecettes(): Observable<Recette[]> {
    return this.http.get<Recette[]>(this.apiUrl);
  }

  /**
   * Récupère une recette par son ID
   */
  getRecetteById(id: number): Observable<Recette> {
    return this.http.get<Recette>(`${this.apiUrl}/${id}`);
  }

  /**
   * Récupère toutes les recettes d'une catégorie
   */
  getRecettesByCategorie(categorieId: number): Observable<Recette[]> {
    return this.http.get<Recette[]>(`${this.apiUrl}/Categorie/${categorieId}`);
  }

  /**
   * Crée une nouvelle recette
   */
  createRecette(recetteData: Partial<Recette>): Observable<Recette> {
    return this.http.post<Recette>(this.apiUrl, recetteData);
  }

  /**
   * Met à jour une recette
   */
  updateRecette(id: number, recetteData: Partial<Recette>): Observable<Recette> {
    return this.http.put<Recette>(`${this.apiUrl}/${id}`, recetteData);
  }

  /**
   * Supprime une recette
   */
  deleteRecette(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  /**
   * Récupère toutes les catégories
   */
  getCategories(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Categories`);
  }

  /**
   * Crée une nouvelle catégorie
   */
  createCategorie(categorieData: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/Categories`, categorieData);
  }

  /**
   * Récupère une catégorie par ID
   */
  getCategorieById(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Categories/${id}`);
  }

  /**
   * Crée une nouvelle étape
   */
  createEtape(etapeData: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/Etapes`, etapeData);
  }

  /**
   * Récupère tous les ingrédients
   */
  getAllIngredients(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Ingredients`);
  }

  /**
   * Crée un nouvel ingrédient
   */
  createIngredient(ingredientData: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/Ingredients`, ingredientData);
  }

  /**
   * Crée une liaison RecetteIngredient
   */
  createRecetteIngredient(recetteIngredientData: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/RecetteIngredients`, recetteIngredientData);
  }

  /**
   * Récupère tous les ingrédients d'une recette
   */
  getRecetteIngredients(recetteId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/RecetteIngredients/Recette/${recetteId}`);
  }

  getFavoris(userId: number) {
  return this.http.get<Recette[]>(`${this.baseUrl}/Utilisateurs/${userId}/favoris`);
}

addFavori(userId: number, recetteId: number) {
  return this.http.post(`${this.baseUrl}/Utilisateurs/${userId}/favoris/${recetteId}`, {});
}

removeFavori(userId: number, recetteId: number) {
  return this.http.delete(`${this.baseUrl}/Utilisateurs/${userId}/favoris/${recetteId}`);
}

}
