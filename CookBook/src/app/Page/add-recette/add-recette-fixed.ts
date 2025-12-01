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
  showCreateCategoryForm = false;
  newCategoryName = '';
  creatingCategory = false;
  imagePreview: string | null = null;
  selectedFile: File | null = null;

  ngOnInit() {
    this.initializeForm();
    // Charger les catégories de test en dur pour les tests
    this.categories = [
      { id: 1, nom_categorie: 'Plat' },
      { id: 2, nom_categorie: 'Dessert' },
      { id: 3, nom_categorie: 'Boisson' },
      { id: 4, nom_categorie: 'Apéritif' },
      { id: 5, nom_categorie: 'Entrée' }
    ];
    this.loadCategories();
    this.loadIngredients();
  }

  private initializeForm() {
    const currentUser = this.authService.currentUserSig();
    this.recetteForm = this.formBuilder.group({
      titre_recette: ['', [Validators.required, Validators.minLength(3)]],
      description_recette: ['', Validators.required],
      photo_recette: [''],
      categorieId: [''],
      etapes: this.formBuilder.array([]),
      ingredients: this.formBuilder.array([])
    });
  }

  private loadCategories() {
    console.log('Chargement des catégories...');
    this.recetteService.getCategories().subscribe({
      next: (data: any) => {
        console.log('✅ Catégories chargées:', data);
        this.categories = data;
        if (!data || data.length === 0) {
          console.warn('⚠️ Aucune catégorie en base de données');
          this.errorMessage = 'Aucune catégorie disponible. Contactez l\'administrateur.';
        } else {
          this.errorMessage = '';
        }
      },
      error: (error: any) => {
        console.error('❌ Erreur lors du chargement des catégories:', error);
        console.error('Statut:', error.status);
        console.error('Message:', error.message);
      }
    });
  }

  private loadIngredients() {
    this.recetteService.getAllIngredients().subscribe({
      next: (data: any) => {
        this.allIngredients = data;
      },
      error: (error: any) => {
        console.error('Erreur lors du chargement des ingrédients:', error);
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

    const categorieIdValue = this.recetteForm.get('categorieId')?.value;
    const recetteData: any = {
      titre_recette: this.recetteForm.get('titre_recette')?.value,
      description_recette: this.recetteForm.get('description_recette')?.value,
      photo_recette: this.recetteForm.get('photo_recette')?.value || '',
      utilisateurId: currentUser.id
    };

    if (categorieIdValue) {
      recetteData.categorieId = parseInt(categorieIdValue);
    }

    console.log('Création de recette avec données:', recetteData);

    this.recetteService.createRecette(recetteData).subscribe({
      next: (response: any) => {
        console.log('Recette créée avec succès:', response);
        const recetteId = response.id;

        const etapesToCreate = this.etapesArray.value.map((etape: any, index: number) => ({
          titre_etape: index + 1,
          description_etape: etape.description_etape,
          id_recette: recetteId
        }));

        const ingredientsToCreate = this.ingredientsArray.value;

        this.createEtapesAndIngredients(recetteId, etapesToCreate, ingredientsToCreate);
      },
      error: (error: any) => {
        this.loading = false;
        console.error('Erreur lors de la création de la recette:', error);
        this.errorMessage = error.error?.message || 'Erreur lors de la création de la recette';
      }
    });
  }

  private createEtapesAndIngredients(recetteId: number, etapes: any[], ingredients: any[]) {
    if (etapes.length === 0 && ingredients.length === 0) {
      this.finishRecetteCreation();
      return;
    }

    if (etapes.length > 0) {
      const etapeRequests = etapes.map(etape => this.recetteService.createEtape(etape));

      forkJoin(etapeRequests).subscribe({
        next: (etapeResponses: any) => {
          console.log('Étapes créées:', etapeResponses);
          this.createIngredientsAndLink(recetteId, ingredients);
        },
        error: (error: any) => {
          this.loading = false;
          console.error('Erreur lors de la création des étapes:', error);
          this.errorMessage = 'Erreur lors de la création des étapes: ' + (error.error?.message || error.message);
        }
      });
    } else {
      this.createIngredientsAndLink(recetteId, ingredients);
    }
  }

  private createIngredientsAndLink(recetteId: number, ingredients: any[]) {
    if (ingredients.length === 0) {
      this.finishRecetteCreation();
      return;
    }

    const ingredientRequests: any[] = [];

    for (const ingredient of ingredients) {
      const existingIngredient = this.allIngredients.find(
        (ing: any) => ing.nom_ingredient.toLowerCase() === ingredient.nom_ingredient.toLowerCase()
      );

      if (existingIngredient) {
        console.log('Ingrédient existant trouvé:', existingIngredient);
        ingredientRequests.push(
          this.recetteService.createRecetteIngredient({
            recetteId: recetteId,
            ingredientId: existingIngredient.id,
            quantite: ingredient.quantite
          })
        );
      } else {
        console.log('Création nouvel ingrédient:', ingredient.nom_ingredient);
        ingredientRequests.push(
          this.recetteService.createIngredient({ nom_ingredient: ingredient.nom_ingredient }).pipe(
            switchMap((newIngredient: any) => {
              console.log('Nouvel ingrédient créé:', newIngredient);
              return this.recetteService.createRecetteIngredient({
                recetteId: recetteId,
                ingredientId: newIngredient.id,
                quantite: ingredient.quantite
              });
            })
          )
        );
      }
    }

    if (ingredientRequests.length > 0) {
      forkJoin(ingredientRequests).subscribe({
        next: (results: any) => {
          console.log('Tous les ingrédients créés et liés:', results);
          this.finishRecetteCreation();
        },
        error: (error: any) => {
          this.loading = false;
          console.error('Erreur lors de la création des ingrédients:', error);
          this.errorMessage = 'Erreur lors de la création des ingrédients: ' + (error.error?.message || error.message);
        }
      });
    } else {
      this.finishRecetteCreation();
    }
  }

  private finishRecetteCreation() {
    this.loading = false;
    this.successMessage = '✅ Recette créée avec succès avec tous les ingrédients et étapes!';
    console.log('Recette complètement créée et sauvegardée en DB');
    this.recetteForm.reset();
    this.etapesArray.clear();
    this.ingredientsArray.clear();

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

  reloadCategories() {
    console.log('Rechargement des catégories...');
    this.loadCategories();
  }

  toggleCreateCategoryForm() {
    this.showCreateCategoryForm = !this.showCreateCategoryForm;
    if (!this.showCreateCategoryForm) {
      this.newCategoryName = '';
    }
  }

  createCategory() {
    if (!this.newCategoryName.trim()) {
      alert('Veuillez entrer un nom de catégorie');
      return;
    }

    this.creatingCategory = true;
    console.log('Création de catégorie:', this.newCategoryName);

    this.recetteService.createCategorie({ nom_categorie: this.newCategoryName }).subscribe({
      next: (response: any) => {
        console.log('✅ Catégorie créée:', response);
        this.categories.push(response);
        this.recetteForm.patchValue({ categorieId: response.id });
        this.newCategoryName = '';
        this.showCreateCategoryForm = false;
        this.creatingCategory = false;
        this.successMessage = `✅ Catégorie "${response.nom_categorie}" créée avec succès!`;
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      },
      error: (error: any) => {
        this.creatingCategory = false;
        console.error('❌ Erreur lors de la création de la catégorie:', error);
        alert('Erreur lors de la création de la catégorie: ' + (error.error?.message || error.message));
      }
    });
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

      console.log('Image sélectionnée:', file.name, file.size);
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
