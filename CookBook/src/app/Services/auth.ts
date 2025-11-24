import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap,Observable  } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { User } from '../Models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  
  
  private apiUrl = 'http://localhost:5230'; 

  // Utilisation des Signals pour l'état utilisateur
  currentUserSig = signal<User | undefined>(undefined);

  constructor() {
    // Au démarrage,  regarde si un token existe
    const token = localStorage.getItem('token');
    if (token) {
      this.decodeToken(token);
    }
  }

 login(Pseudo: string, MotDePasse: string): Observable<string> {
  const body = new URLSearchParams();
  body.set('Pseudo', Pseudo);
  body.set('MotDePasse', MotDePasse);

  return this.http.post(
    `${this.apiUrl}/login`,
    body.toString(),
    {
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      },
      responseType: 'text'
    }
  ).pipe(
    tap((token) => {
      localStorage.setItem('token', token);
      this.decodeToken(token);
    })
  );
}

  register(user: Partial<User> & { motDePasse: string }) {
    
    const payload = {
      pseudo: user.pseudo,
      email: user.email,
      motDePasse: user.motDePasse,
      roleId: user.roleId || 1 // Par défaut roleId 1 
    };
    return this.http.post(`${this.apiUrl}/api/Utilisateurs`, payload);
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSig.set(undefined);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  private decodeToken(token: string) {
    try {
      const decoded: any = jwtDecode(token);
      
      // MAPPING  correspondre les Claims du C# avec l'objet Angular
      const user: User = {
        id: Number(decoded.id), 
        pseudo: decoded.unique_name || decoded.name, // ClaimTypes.Name
        email: decoded.email, // ClaimTypes.Email
        roleId: 0, 
        role: { id: 0, nom_role: decoded.role } // ClaimTypes.Role
      };
      
      this.currentUserSig.set(user);
    } catch (e) {
      // Si token invalide ou expiré
      this.logout();
    }
  }


 

  /**Recup tous les favoris d'un utilisateur **/
  getAllFavoriteRecettes(id: number): Observable<any[]> {
      return this.http.get<any[]>(`${this.apiUrl}/api/Utilisateurs/${id}/favoris`);
  }
}