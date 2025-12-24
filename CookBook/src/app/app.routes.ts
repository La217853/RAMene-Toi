import { Routes } from '@angular/router';
import { LoginComponent } from './Page/login/login/login';
import { RegisterComponent } from './Page/register/register/register';
import { ProfileComponent } from './Page/profile/profile/profile';
import { DashboardComponent } from './Page/dashboard/dashboard';
import { AdminUsersComponent } from './Page/admin-users/admin-users';
import { AddRecetteComponent } from './Page/add-recette/add-recette';
import { inject } from '@angular/core';
import { AuthService } from './Services/auth';
import { RecetteDetailsComponent } from './Page/details-recette/details-recette';
import { FavoritesComponent } from './Page/favorites/favorites';
import { MyRecipesComponent } from './Page/my-recipes/my-recipes';
import { DecouvrirComponent } from './Page/decouvrir/decouvrir';


const authGuard = () => {
  const authService = inject(AuthService);
  // Si pas de token, le service auth renvoie false/null, donc on redirige
  if (!authService.getToken()) {
     authService.logout(); // Redirige vers login
     return false;
  }
  return true;
};

const adminGuard = () => {
  const authService = inject(AuthService);
  const currentUser = authService.currentUserSig();

  if (!authService.getToken()) {
    authService.logout();
    return false;
  }

  // Vérifier si l'utilisateur est admin
  if (currentUser?.role?.nom_role !== 'Admin') {
    return false;
  }

  return true;
};

export const routes: Routes = [
  { path: '', redirectTo: 'decouvrir', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  {
    path: 'favoris',
    component: FavoritesComponent,
    canActivate: [authGuard]
  },
  {
    path: 'mes-recettes',
    component: MyRecipesComponent,
    canActivate: [authGuard]
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [authGuard]
  },
  {
    path: 'add-recette',
    component: AddRecetteComponent,
    canActivate: [authGuard]
  },
  {
    path: 'admin/users',
    component: AdminUsersComponent,
    canActivate: [adminGuard] //admin 
  },
  { 
    path: 'recette/:id',
    component: RecetteDetailsComponent,
    canActivate: [authGuard] 
  },
  {
    path: 'decouvrir',
    component: DecouvrirComponent,
    canActivate: [authGuard]
  }
  
];