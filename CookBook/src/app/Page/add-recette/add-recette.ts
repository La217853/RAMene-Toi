import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RecetteService } from '../../Services/recette.service';
import { AuthService } from '../../Services/auth';
import { forkJoin, switchMap, of } from 'rxjs';

@Component({
  selector: 'app-add-recette',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
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
  allIngredients: any[] = [];
  imagePreview: string | null = null;
  selectedFile: File | null = null;

  ngOnInit() {
    this.initializeForm();
    this.loadCategories();
    this.loadIngredients();
  }

  private initializeForm() {
    const currentUser = this.authService.currentUserSig();
    this.recetteForm = this.formBuilder.group({
      titre_recette: ['', [Validators.required, Validators.minLength(3)]],
      description_recette: ['', Validators.required],
      photo_recette: [''],
      categorieId: ['', Validators.required],
      etapes: this.formBuilder.array([]),
      ingredients: this.formBuilder.array([])
    });
  }

  loadCategories() {
    this.recetteService.getCategories().subscribe({
      next: (data: any) => {
        this.categories = data || [];
        if (!data || data.length === 0) {
          this.errorMessage = 'Aucune catégorie disponible. Veuillez contacter l\'administrateur.';
        } else {
          this.errorMessage = '';
        }
      },
      error: (error: any) => {
        this.errorMessage = 'Impossible de charger les catégories. Vérifiez votre connexion au serveur.';
        this.categories = [];
      }
    });
  }

  private loadIngredients() {
    this.recetteService.getAllIngredients().subscribe({
      next: (data: any) => {
        this.allIngredients = data || [];
      },
      error: (error: any) => {
        this.allIngredients = [];
      }
    });
  }

  get etapesArray(): FormArray {
    return this.recetteForm.get('etapes') as FormArray;
  }

  get ingredientsArray(): FormArray {
    return this.recetteForm.get('ingredients') as FormArray;
  }

  addEtape() {
    const etapeForm = this.formBuilder.group({
      titre_etape: ['', Validators.required],
      description_etape: ['', Validators.required]
    });
    this.etapesArray.push(etapeForm);
  }

  removeEtape(index: number) {
    this.etapesArray.removeAt(index);
  }

  addIngredient() {
    const ingredientForm = this.formBuilder.group({
      nom_ingredient: ['', Validators.required],
      quantite: ['', Validators.required]
    });
    this.ingredientsArray.push(ingredientForm);
  }

  removeIngredient(index: number) {
    this.ingredientsArray.removeAt(index);
  }

  onSubmit() {
    this.submitted = true;
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.recetteForm.get('titre_recette')?.value || this.recetteForm.get('titre_recette')?.value.trim() === '') {
      this.errorMessage = 'Le titre de la recette est obligatoire';
      return;
    }

    if (!this.recetteForm.get('description_recette')?.value || this.recetteForm.get('description_recette')?.value.trim() === '') {
      this.errorMessage = 'La description est obligatoire';
      return;
    }

    if (!this.recetteForm.get('categorieId')?.value) {
      this.errorMessage = 'Veuillez sélectionner une catégorie';
      return;
    }

    this.loading = true;

    const currentUser = this.authService.currentUserSig();
    if (!currentUser) {
      this.errorMessage = 'Utilisateur non identifié';
      this.loading = false;
      return;
    }

    const categorieIdValue = this.recetteForm.get('categorieId')?.value;
    const recetteData: any = {
      titre_recette: this.recetteForm.get('titre_recette')?.value.trim(),
      description_recette: this.recetteForm.get('description_recette')?.value.trim(),
      photo_recette: this.recetteForm.get('photo_recette')?.value || '',
      utilisateurId: currentUser.id
    };

    if (categorieIdValue) {
      recetteData.categorieId = parseInt(categorieIdValue);
    }

    this.recetteService.createRecette(recetteData).subscribe({
      next: (response: any) => {
        const recetteId = response.id;

        const etapesToCreate = this.etapesArray.value.map((etape: any, index: number) => {
          const etapeData = {
            titre_etape: (index + 1).toString(),
            description_etape: etape.description_etape.trim(),
            recetteId: recetteId
          };
          return etapeData;
        });

        const ingredientsToCreate = this.ingredientsArray.value.map((ing: any) => ({
          nom_ingredient: ing.nom_ingredient.trim(),
          quantite: ing.quantite.trim()
        }));

        this.createEtapesAndIngredients(recetteId, etapesToCreate, ingredientsToCreate);
      },
      error: (error: any) => {
        this.loading = false;
        this.errorMessage = error.error?.message || 'Erreur lors de la création de la recette: ' + error.message;
      }
    });
  }

  private createEtapesAndIngredients(recetteId: number, etapes: any[], ingredients: any[]) {
    if (etapes.length === 0 && ingredients.length === 0) {
      this.finishRecetteCreation();
      return;
    }

    if (etapes.length > 0) {
      this.createEtapesSequentially(etapes, 0, recetteId, ingredients);
    } else {
      this.createIngredientsAndLink(recetteId, ingredients);
    }
  }

  private createEtapesSequentially(etapes: any[], index: number, recetteId: number, ingredients: any[]) {
    if (index >= etapes.length) {
      this.createIngredientsAndLink(recetteId, ingredients);
      return;
    }

    const etape = etapes[index];

    this.recetteService.createEtape(etape).subscribe({
      next: (response: any) => {
        this.createEtapesSequentially(etapes, index + 1, recetteId, ingredients);
      },
      error: (error: any) => {
        this.loading = false;
        
        let errorDetails = `Erreur lors de la création de l'étape ${index + 1}`;
        if (error.error && typeof error.error === 'string') {
          errorDetails = error.error;
        } else if (error.message) {
          errorDetails = error.message;
        }
        
        this.errorMessage = errorDetails;
      }
    });
  }

  private createIngredientsAndLink(recetteId: number, ingredients: any[]) {
    if (ingredients.length === 0) {
      this.finishRecetteCreation();
      return;
    }

    this.createIngredientsSequentially(ingredients, 0, recetteId);
  }

  private createIngredientsSequentially(ingredients: any[], index: number, recetteId: number) {
    if (index >= ingredients.length) {
      this.finishRecetteCreation();
      return;
    }

    const ingredient = ingredients[index];

    const existingIngredient = this.allIngredients.find(
      (ing: any) => ing.nom_ingredient.toLowerCase() === ingredient.nom_ingredient.toLowerCase()
    );

    if (existingIngredient) {
      this.recetteService.createRecetteIngredient({
        recetteId: recetteId,
        ingredientId: existingIngredient.id,
        quantite: ingredient.quantite
      }).subscribe({
        next: (response: any) => {
          this.createIngredientsSequentially(ingredients, index + 1, recetteId);
        },
        error: (error: any) => {
          this.loading = false;
          
          let errorDetails = `Erreur lors de la liaison de l'ingrédient ${index + 1}`;
          if (error.error && typeof error.error === 'string') {
            errorDetails = error.error;
          } else if (error.message) {
            errorDetails = error.message;
          }
          
          this.errorMessage = errorDetails;
        }
      });
    } else {
      this.recetteService.createIngredient({ nom_ingredient: ingredient.nom_ingredient }).subscribe({
        next: (newIngredient: any) => {
          this.recetteService.createRecetteIngredient({
            recetteId: recetteId,
            ingredientId: newIngredient.id,
            quantite: ingredient.quantite
          }).subscribe({
            next: (linkResponse: any) => {
              this.createIngredientsSequentially(ingredients, index + 1, recetteId);
            },
            error: (linkError: any) => {
              this.loading = false;
              
              let errorDetails = `Erreur lors de la liaison de l'ingrédient ${index + 1}`;
              if (linkError.error && typeof linkError.error === 'string') {
                errorDetails = linkError.error;
              } else if (linkError.message) {
                errorDetails = linkError.message;
              }
              
              this.errorMessage = errorDetails;
            }
          });
        },
        error: (error: any) => {
          this.loading = false;
          
          let errorDetails = `Erreur lors de la création de l'ingrédient ${index + 1}`;
          if (error.error && typeof error.error === 'string') {
            errorDetails = error.error;
          } else if (error.message) {
            errorDetails = error.message;
          }
          
          this.errorMessage = errorDetails;
        }
      });
    }
  }

  private finishRecetteCreation() {
    this.loading = false;
    this.successMessage = '✅ Recette créée avec succès avec tous les ingrédients et étapes!';
    this.recetteForm.reset();
    this.etapesArray.clear();
    this.ingredientsArray.clear();
    this.imagePreview = null;
    this.selectedFile = null;

    setTimeout(() => {
      this.router.navigate(['/dashboard']);
    }, 2000);
  }

  get titre_recette() {
    return this.recetteForm.get('titre_recette');
  }

  get description_recette() {
    return this.recetteForm.get('description_recette');
  }

  get categorieId() {
    return this.recetteForm.get('categorieId');
  }

  cancel() {
    this.router.navigate(['/dashboard']);
  }

  onImageSelected(event: any) {
    const file: File = event.target.files[0];
    
    if (file) {
      if (!file.type.startsWith('image/')) {
        alert('Veuillez sélectionner une image valide');
        return;
      }

      const maxSize = 5 * 1024 * 1024;
      if (file.size > maxSize) {
        alert('L\'image ne doit pas dépasser 5MB');
        return;
      }

      this.selectedFile = file;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.imagePreview = e.target.result;
        this.recetteForm.patchValue({ photo_recette: file.name });
      };
      reader.readAsDataURL(file);
    }
  }

  removeImage() {
    this.imagePreview = null;
    this.selectedFile = null;
    this.recetteForm.patchValue({ photo_recette: '' });
    const fileInput = document.getElementById('photo_file') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }
}