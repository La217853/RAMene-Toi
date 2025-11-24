import { Component, inject, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../Services/auth';
import { RecetteService } from '../../../Services/recette.service';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class ProfileComponent {

message = signal<string | null>(null);
messageType = signal<'success' | 'error' | null>(null);


  authService = inject(AuthService);
  recetteService = inject(RecetteService);
  user = this.authService.currentUserSig;
  recettesCount = signal<number>(0);
  favoritesCount = signal<number>(0);
  fb = inject(FormBuilder);
  isEditMode = signal<boolean>(false);

  //Pour le form de modification
    profilForm = this.fb.group({
    pseudo: ['',[Validators.required]],
    email: ['',[Validators.required, Validators.email]]
  });


  constructor() {
    // Charger le nombre de recettes quand l'utilisateur est disponible
    effect(() => {
      const currentUser = this.user();

      if (currentUser?.id) {
        this.loadRecettesCount(currentUser.id);
        this.loadFavoriteCount(currentUser.id);

        this.profilForm.patchValue({
          pseudo: currentUser.pseudo,
          email: currentUser.email
        });
      }
    });
  }




  private loadRecettesCount(userId: number) {
    this.recetteService.getRecettesByUtilisateur(userId).subscribe({
      next: (recettes) => {
        this.recettesCount.set(recettes.length);
      },
      error: (error) => {
        // console.error('Erreur lors du chargement des recettes:', error);
        this.recettesCount.set(0);
      }
    });
  }

   private loadFavoriteCount(userId: number) {
    this.authService.getAllFavoriteRecettes(userId).subscribe({
      next: (favoris) => {
        this.favoritesCount.set(favoris.length);
      },
      error: (error) => {
      //  console.error('Erreur lors du chargement des favoris:', error);
        this.favoritesCount.set(0);
      }
    });
  }

  toggleEditMode() {
    this.isEditMode.set(!this.isEditMode());

    // Si active le mode modif,  remplit le formulaire avec les données actuelles
    if (this.isEditMode() && this.user()) {
      const currentUser = this.user();
      this.profilForm.patchValue({
        pseudo: currentUser?.pseudo,
        email: currentUser?.email
      });
    }
  }

  cancelEdit() {
    this.isEditMode.set(false);
    this.profilForm.reset();
  }

   PutProfile(userId: number) {
    if(!userId) return;
      
    if (this.profilForm.invalid) {
      this.profilForm.markAllAsTouched();
      this.setMessage("error","Veuillez corriger les erreurs du formulaire.");
      return;
    }

    const formValue = this.profilForm.value;

    // Récupérer l'utilisateur complet pour avoir tous les champs requis par le backend
    this.authService.getUserById(userId).subscribe({
      next: (fullUser) => {

      const pseudoIdentique = formValue.pseudo === fullUser.pseudo;
      const emailIdentique = formValue.email === fullUser.email;

      if (pseudoIdentique && emailIdentique) {
        this.setMessage("error","Aucune modification détectée (pseudo et e-mail identiques).");
        return;
      }
        // Construire l'objet avec TOUS les champs requis
        const updatedData: any = {  
          id: fullUser.id,
          pseudo: formValue.pseudo,
          email: formValue.email,
          roleId: fullUser.roleId,
          motDePasse: '' // vide pour pas modif le MDP sinon ça change et le user ne pourra plus se co
        };

        this.authService.PutProfile(userId, updatedData).subscribe({
          next: (res) => {
            // Mettre à jour le signal utilisateur
            const currentUser = this.authService.currentUserSig();
            if (currentUser) {
              currentUser.pseudo = formValue.pseudo || currentUser.pseudo;
              currentUser.email = formValue.email || currentUser.email;
              this.authService.currentUserSig.set({...currentUser});
            }
            this.setMessage("success","Profil mis à jour avec succès. Déconnectez-vous et reconnectez-vous pour voir les changements.");
            this.profilForm.reset();
          },
          error: (err) => { 
            console.error('Erreur lors de la mise à jour du profil:', err);
          this.messageType.set('error');
          this.setMessage("error","Erreur lors de la mise à jour du profil. Veuillez réessayer.");
          }
        });
      },
      error: (err) => {
        console.error('Erreur lors de la récupération du profil:', err);
      this.message.set("Impossible de récupérer les données du profil.");
      }
    });
  }

  private setMessage(type: 'success' | 'error', text: string) {
  this.messageType.set(type);
  this.message.set(text);

  // Disparition au bout de 5 secondes
  setTimeout(() => {
    this.message.set(null);
    this.messageType.set(null);
  }, 5000);
}
}
