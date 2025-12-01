import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../Services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  errorMessage = '';
  successMessage = '';


  registerForm = this.fb.group({
    pseudo: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    motDePasse: ['', [Validators.required, Validators.minLength(6)]],
    roleId: [1] // 1 defaut pour user
  });

  onSubmit() {
    if (this.registerForm.valid) {
      // cast any pour ok objet attendu
      this.authService.register(this.registerForm.value as any).subscribe({
        next: () => {
          this.successMessage = 'Compte créé avec succès ! Redirection...';
          this.errorMessage = '';
          // Redirection après 1 secondes
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 1000);
        },
        error: (err) => {
          console.error(err);
          this.successMessage = '';

          // Gestion d'erreurs
          if (err.status === 409 || err.status === 400) {
          
            if (err.error?.message) {
              this.errorMessage = err.error.message;
            } else {
              this.errorMessage = 'Cet email ou ce pseudo est déjà utilisé.';
            }
          } else if (err.status === 500) {
            this.errorMessage = 'Erreur du serveur. Veuillez réessayer plus tard.';
          } else {
            this.errorMessage = 'Erreur lors de l\'inscription. Veuillez réessayer.';
          }
        }
      });
    }
  }
}