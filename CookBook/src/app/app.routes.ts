import { Routes } from '@angular/router';
import { LoginComponent } from './Page/login/login/login';
import { RegisterComponent } from './Page/register/register/register';
import { ProfileComponent } from './Page/profile/profile/profile';
import { DashboardComponent } from './Page/dashboard/dashboard';
import { inject } from '@angular/core';
import { AuthService } from './Services/auth';
import { RecetteDetailsComponent } from './Page/details-recette/details-recette';


const authGuard = () => {
  const authService = inject(AuthService);
  // Si pas de token, le service auth renvoie false/null, donc on redirige
  if (!authService.getToken()) {
     authService.logout(); // Redirige vers login
     return false;
  }
  return true;
};

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard] // Protection de la route
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [authGuard] 
  },
  { path: 'recette/:id',
    component: RecetteDetailsComponent,
    canActivate: [authGuard] 
  }
  
];