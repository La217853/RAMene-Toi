import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RecetteService } from '../../Services/recette.service';
import { AuthService } from '../../Services/auth';

@Component({
  selector: 'app-add-recette',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-recette.html',
  styleUrl: './add-recette.css'
})
export class AddRecetteComponent implements OnInit {
  private formBuilder = inject(FormBuilder);
  private recetteService = inject(RecetteService);
  private authService = inject(AuthService);
  private router = inject(Router);

  recetteForm!: FormGroup;
  categories: any[] = [];
  loading = false;
  submitted = false;
  errorMessage = '';
  successMessage = '';

  ngOnInit() {
    this.initializeForm();
    this.loadCategories();
  }

  private initializeForm() {
    const currentUser = this.authService.currentUserSig();
    this.recetteForm = this.formBuilder.group({
      titre: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', Validators.required],
      tempsPreparation: ['', [Validators.required, Validators.min(0)]],
      tempsCuisson: ['', [Validators.required, Validators.min(0)]],
      difficulte: ['Facile', Validators.required],
      portions: ['', [Validators.required, Validators.min(1)]],
      categorieId: ['', Validators.required],
      photo: ['']
    });
  }

  private loadCategories() {
    this.recetteService.getCategories().subscribe({
      next: (data) => {
        this.categories = data;
      },
      error: (error) => {
        console.error('Erreur lors du chargement des catégories:', error);
        this.errorMessage = 'Impossible de charger les catégories';
      }
    });
  }

  onSubmit() {
    this.submitted = true;
    this.errorMessage = '';
    this.successMessage = '';

    if (this.recetteForm.invalid) {
      this.errorMessage = 'Veuillez remplir tous les champs obligatoires';
      return;
    }

    this.loading = true;

    const currentUser = this.authService.currentUserSig();
    if (!currentUser) {
      this.errorMessage = 'Utilisateur non identifié';
      this.loading = false;
      return;
    }

    const recetteData = {
      ...this.recetteForm.value,
      utilisateurId: currentUser.id,
      categorieId: parseInt(this.recetteForm.value.categorieId)
    };

    this.recetteService.createRecette(recetteData).subscribe({
      next: (response) => {
        this.loading = false;
        this.successMessage = 'Recette créée avec succès!';
        this.recetteForm.reset();
        
        // Redirection après 2 secondes
        setTimeout(() => {
          this.router.navigate(['/dashboard']);
        }, 2000);
      },
      error: (error) => {
        this.loading = false;
        console.error('Erreur lors de la création de la recette:', error);
        this.errorMessage = error.error?.message || 'Erreur lors de la création de la recette';
      }
    });
  }

  get titre() {
    return this.recetteForm.get('titre');
  }

  get description() {
    return this.recetteForm.get('description');
  }

  get tempsPreparation() {
    return this.recetteForm.get('tempsPreparation');
  }

  get tempsCuisson() {
    return this.recetteForm.get('tempsCuisson');
  }

  get difficulte() {
    return this.recetteForm.get('difficulte');
  }

  get portions() {
    return this.recetteForm.get('portions');
  }

  get categorieId() {
    return this.recetteForm.get('categorieId');
  }

  cancel() {
    this.router.navigate(['/dashboard']);
  }
}