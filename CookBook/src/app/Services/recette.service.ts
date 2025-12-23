import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Recette } from '../Models/recette.model';

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

  /**
<<<<<<< HEAD
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
=======
>>>>>>> 7e09d5e920580196c6a58ced267b83341fc08fd5
   * Récupère toutes les recettes
   */
  getAllRecettes(): Observable<Recette[]> {
    return this.http.get<Recette[]>(this.apiUrl);
  }

  /**
<<<<<<< HEAD
   * Récupère une recette par son ID
=======
   * Récupère une recette par ID
>>>>>>> 7e09d5e920580196c6a58ced267b83341fc08fd5
   */
  getRecetteById(id: number): Observable<Recette> {
    return this.http.get<Recette>(`${this.apiUrl}/${id}`);
  }

  /**
<<<<<<< HEAD
   * Crée une nouvelle recette
   */
  createRecette(recette: Partial<Recette>): Observable<Recette> {
    return this.http.post<Recette>(this.apiUrl, recette);
=======
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
>>>>>>> 7e09d5e920580196c6a58ced267b83341fc08fd5
  }

  /**
   * Met à jour une recette
   */
<<<<<<< HEAD
  updateRecette(id: number, recette: Partial<Recette>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, recette);
=======
  updateRecette(id: number, recetteData: Partial<Recette>): Observable<Recette> {
    return this.http.put<Recette>(`${this.apiUrl}/${id}`, recetteData);
>>>>>>> 7e09d5e920580196c6a58ced267b83341fc08fd5
  }

  /**
   * Supprime une recette
   */
  deleteRecette(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
<<<<<<< HEAD
=======

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
>>>>>>> 7e09d5e920580196c6a58ced267b83341fc08fd5
}