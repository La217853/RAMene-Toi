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

  /**
   * Récupère toutes les recettes d'un utilisateur
   */
  getRecettesByUtilisateur(utilisateurId: number): Observable<Recette[]> {
    return this.http.get<Recette[]>(`${this.apiUrl}/Utilisateur/${utilisateurId}`);
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
   * Crée une nouvelle recette
   */
  createRecette(recette: Partial<Recette>): Observable<Recette> {
    return this.http.post<Recette>(this.apiUrl, recette);
  }

  /**
   * Met à jour une recette
   */
  updateRecette(id: number, recette: Partial<Recette>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, recette);
  }

  /**
   * Supprime une recette
   */
  deleteRecette(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}