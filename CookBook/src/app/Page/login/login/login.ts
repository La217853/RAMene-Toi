import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../Services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.html', 
  styleUrl: './login.css'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  errorMessage = '';

  loginForm = this.fb.group({
    Pseudo: ['', [Validators.required]],
    MotDePasse: ['', Validators.required]
  });

  onSubmit() {
    if (this.loginForm.valid) {
      const { Pseudo, MotDePasse } = this.loginForm.value;
      
      this.authService.login(Pseudo!, MotDePasse!).subscribe({
        next: () => {
          this.router.navigate(['/decouvrir']);
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Pseudo ou mot de passe incorrect.';
        }
      });
    }
  }
}