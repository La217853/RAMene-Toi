import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../Services/auth';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { RecetteService } from '../../Services/recette.service';
import { User } from '../../Models/user.model';
import { Role } from '../../Models/user.model';


@Component({

  selector: 'app-admin-users',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './admin-users.html',
  styleUrl: './admin-users.css'
})
export class AdminUsersComponent implements OnInit {
  authService = inject(AuthService);
  fb = inject(FormBuilder);

  users = signal<User[]>([]);
  roles = signal<Role[]>([]);
  isLoading = signal<boolean>(false);
  message = signal<string | null>(null);
  messageType = signal<'success' | 'error' | null>(null);
  editingUserId = signal<number | null>(null);
  recetteService = inject(RecetteService);
  // Formulaire d'édition
  editForm = this.fb.group({
    pseudo: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    roleId: [1, [Validators.required]]
  });

  ngOnInit() {
    this.loadUsers();
    this.loadRoles();
  }

  loadUsers() {
    this.isLoading.set(true);
    this.authService.getAllUsers().subscribe({
      next: (users) => {
        console.log('Utilisateurs chargés:', users);

      
       users.forEach(user => {
          this.recetteService.getRecettesByUtilisateur(user.id).subscribe({
            next: (recettes) => {
              user.recettesCount = recettes.length;
          },
         error: () => {
              user.recettesCount = 0;
            }
          });
        });

        this.users.set(users);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Erreur lors du chargement des utilisateurs:', err);
        this.setMessage('error', 'Erreur lors du chargement des utilisateurs.');
        this.isLoading.set(false);
      }
    });
  }

  loadRoles() {
    this.authService.getAllRoles().subscribe({
      next: (roles) => {
        console.log('Rôles chargés:', roles);
        this.roles.set(roles);
      },
      error: (err) => {
        console.error('Erreur lors du chargement des rôles:', err);
        this.setMessage('error', 'Erreur lors du chargement des rôles.');
      }
    });
  }

  startEdit(user: User) {
    this.editingUserId.set(user.id);
    this.editForm.patchValue({
      pseudo: user.pseudo,
      email: user.email,
      roleId: user.roleId
    });
  }
  
getRoleName(roleId: number): string {
  const role = this.roles().find(r => r.id === roleId);
  return role?.nom_role ?? 'Utilisateur';
}

  cancelEdit() {
    this.editingUserId.set(null);
    this.editForm.reset();
  }

  saveUser(userId: number) {
    if (this.editForm.invalid) {
      this.editForm.markAllAsTouched();
      this.setMessage('error', 'Veuillez corriger les erreurs du formulaire.');
      return;
    }

    const formValue = this.editForm.value;
    const updatedData = {
      id: userId,
      pseudo: formValue.pseudo!,
      email: formValue.email!,
      roleId: formValue.roleId!,
      motDePasse: '' // Vide pour ne pas modifier le mot de passe sinon c cuit
    };

    this.authService.PutProfile(userId, updatedData).subscribe({
      next: () => {
        this.setMessage('success', 'Utilisateur mis à jour avec succès.');
        this.editingUserId.set(null);
        this.loadUsers();
      },
      error: (err) => {
        //console.error('Erreur lors de la mise à jour:', err);
        this.setMessage('error', 'Erreur lors de la mise à jour de l\'utilisateur.');
      }
    });
  }

  deleteUser(userId: number, pseudo: string) {
    const currentUser = this.authService.currentUserSig();
    if (currentUser?.id === userId) {
      this.setMessage('error', 'Vous ne pouvez pas supprimer votre propre compte.');
      return;
    }

    if (!confirm(`Êtes-vous sûr de vouloir supprimer l'utilisateur "${pseudo}" ?`)) {
      return;
    }

    this.authService.deleteUser(userId).subscribe({
      next: () => {
        this.setMessage('success', `Utilisateur "${pseudo}" supprimé avec succès.`);
        this.loadUsers();
      },
      error: (err) => {
      //  console.error('Erreur lors de la suppression:', err);
        this.setMessage('error', 'Erreur lors de la suppression de l\'utilisateur.');
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

  //test current user
  isCurrentUser(userId: User): boolean {
    return userId.id === this.authService.currentUserSig()?.id;
  }

   
  
 
}
