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
}