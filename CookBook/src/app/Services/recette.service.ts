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
  private recettesUrl = 'http://localhost:5230/api/Recettes';
  private categoriesUrl = 'http://localhost:5230/api/Categories';
 private utilisateursUrl = 'http://localhost:5230/api/Utilisateurs';
 private recettesIngredientUrl = 'http://localhost:5230/api/RecetteIngredients/Recette'
 private etapeUrl = 'http://localhost:5230/api/Etapes'
 

  /**
   * Récupère une recette par ID
   */
  getRecette(id: number): Observable<Recette> {
    return this.http.get<Recette>(`${this.recettesUrl}/${id}`);
  }

  /**
   * Récupère toutes les recettes
   */
  getRecettes(): Observable<Recette[]> {
    return this.http.get<Recette[]>(this.recettesUrl);
  }

  /**
   * Récupère toutes les recettes d'un utilisateur
   */
  getRecettesByUtilisateur(utilisateurId: number): Observable<Recette[]> {
    return this.http.get<Recette[]>(`${this.recettesUrl}/Utilisateur/${utilisateurId}`);
  }

  /**
   * Crée une nouvelle recette
   */
  createRecette(recette: Recette): Observable<Recette> {
    return this.http.post<Recette>(this.recettesUrl, recette);
  }

  /**
   * Met à jour une recette
   */
  updateRecette(id: number, recette: Recette): Observable<Recette> {
    return this.http.put<Recette>(`${this.recettesUrl}/${id}`, recette);
  }

  /**
   * Supprime une recette
   */
  deleteRecette(id: number): Observable<void> {
    return this.http.delete<void>(`${this.recettesUrl}/${id}`);
  }


  getIngredients(id: number): Observable<IngredientQuantite[]> { 
    return this.http.get<IngredientQuantite[]>(`${this.recettesIngredientUrl}/${id}`); 
  } 
  
  getEtapes(id: number): Observable<Etape[]> { 
    return this.http.get<Etape[]>(`${this.etapeUrl}/${id}`);
  }
  
  getCategorie(id: number): Observable<Categorie> { 
    return this.http.get<Categorie>(`${this.categoriesUrl}/${id}`); 
  }
  
  getAuteur(id: number): Observable<User> { 
    return this.http.get<User>(`${this.utilisateursUrl}/${id}`); 
  }

  getIngredient(id: number) { 
    return this.http.get<Ingredient>(`http://localhost:5230/api/Ingredients/${id}`); 
  }

}