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
    console.log('🚀 Initialisation du composant add-recette');
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
    console.log('✅ Formulaire initialisé');
  }

  loadCategories() {
    console.log('🔄 Chargement des catégories...');
    this.recetteService.getCategories().subscribe({
      next: (data: any) => {
        console.log('✅ Catégories chargées:', data);
        console.log('Nombre de catégories:', data ? data.length : 0);
        this.categories = data || [];
        if (!data || data.length === 0) {
          console.warn('⚠️ Aucune catégorie disponible');
          this.errorMessage = 'Aucune catégorie disponible. Veuillez contacter l\'administrateur.';
        } else {
          console.log('✓ Catégories disponibles:', this.categories.map((c: any) => c.nom_categorie));
          this.errorMessage = '';
        }
      },
      error: (error: any) => {
        console.error('❌ ERREUR lors du chargement des catégories:', error);
        console.error('Status:', error.status);
        console.error('Message:', error.message);
        console.error('Response:', error.error);
        this.errorMessage = 'Impossible de charger les catégories. Vérifiez votre connexion au serveur.';
        this.categories = [];
      }
    });
  }

  private loadIngredients() {
    console.log('🔄 Chargement des ingrédients...');
    this.recetteService.getAllIngredients().subscribe({
      next: (data: any) => {
        console.log('✅ Ingrédients chargés:', data);
        console.log('Nombre d\'ingrédients:', data ? data.length : 0);
        this.allIngredients = data || [];
      },
      error: (error: any) => {
        console.error('❌ ERREUR lors du chargement des ingrédients:', error);
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

    console.log('🔍 Vérification du formulaire...');
    console.log('Titre:', this.recetteForm.get('titre_recette')?.value);
    console.log('Description:', this.recetteForm.get('description_recette')?.value);
    console.log('Catégorie:', this.recetteForm.get('categorieId')?.value);
    console.log('Formulaire valide?:', this.recetteForm.valid);
    console.log('Erreurs formulaire:', this.recetteForm.errors);

    // Vérifier chaque champ
    if (!this.recetteForm.get('titre_recette')?.value || this.recetteForm.get('titre_recette')?.value.trim() === '') {
      this.errorMessage = 'Le titre de la recette est obligatoire';
      console.error('❌ Titre manquant');
      return;
    }

    if (!this.recetteForm.get('description_recette')?.value || this.recetteForm.get('description_recette')?.value.trim() === '') {
      this.errorMessage = 'La description est obligatoire';
      console.error('❌ Description manquante');
      return;
    }

    if (!this.recetteForm.get('categorieId')?.value) {
      this.errorMessage = 'Veuillez sélectionner une catégorie';
      console.error('❌ Catégorie non sélectionnée');
      return;
    }

    this.loading = true;

    const currentUser = this.authService.currentUserSig();
    if (!currentUser) {
      this.errorMessage = 'Utilisateur non identifié';
      console.error('❌ Pas d\'utilisateur courant');
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

    // Ajouter categorieId seulement s'il existe
    if (categorieIdValue) {
      recetteData.categorieId = parseInt(categorieIdValue);
    }

    console.log('🔄 Début de création de recette...');
    console.log('📋 Données recette:', recetteData);

    // Créer la recette d'abord
    this.recetteService.createRecette(recetteData).subscribe({
      next: (response: any) => {
        console.log('✅ Recette créée avec succès! ID:', response.id);
        const recetteId = response.id;

        // Préparer les étapes
        const etapesToCreate = this.etapesArray.value.map((etape: any, index: number) => {
          const etapeData = {
            titre_etape: index + 1,
            description_etape: etape.description_etape.trim(),
            id_recette: recetteId
          };
          console.log('📝 Étape', index + 1, ':', etapeData);
          return etapeData;
        });

        // Préparer les ingrédients
        const ingredientsToCreate = this.ingredientsArray.value.map((ing: any) => ({
          nom_ingredient: ing.nom_ingredient.trim(),
          quantite: ing.quantite.trim()
        }));

        console.log('📝 Étapes à créer:', etapesToCreate.length);
        console.log('Détail étapes:', JSON.stringify(etapesToCreate, null, 2));
        console.log('🥘 Ingrédients à créer:', ingredientsToCreate.length);

        // Appeler la méthode pour créer étapes et ingrédients
        this.createEtapesAndIngredients(recetteId, etapesToCreate, ingredientsToCreate);
      },
      error: (error: any) => {
        this.loading = false;
        console.error('❌ ERREUR lors de la création de la recette:', error);
        console.error('Status:', error.status);
        console.error('Message:', error.message);
        console.error('Response:', error.error);
        this.errorMessage = error.error?.message || 'Erreur lors de la création de la recette: ' + error.message;
      }
    });
  }

  private createEtapesAndIngredients(recetteId: number, etapes: any[], ingredients: any[]) {
    // Si pas d'étapes et pas d'ingrédients, considérer comme succès
    if (etapes.length === 0 && ingredients.length === 0) {
      console.log('✅ Pas d\'étapes ni d\'ingrédients - recette créée directement');
      this.finishRecetteCreation();
      return;
    }

    // Créer les étapes une par une de manière séquentielle
    if (etapes.length > 0) {
      console.log('🔄 Création de', etapes.length, 'étape(s) de manière séquentielle...');
      
      this.createEtapesSequentially(etapes, 0, recetteId, ingredients);
    } else {
      // Pas d'étapes, passer directement aux ingrédients
      console.log('⏭️ Pas d\'étapes - passage direct aux ingrédients');
      this.createIngredientsAndLink(recetteId, ingredients);
    }
  }

  private createEtapesSequentially(etapes: any[], index: number, recetteId: number, ingredients: any[]) {
    if (index >= etapes.length) {
      // Toutes les étapes sont créées, passer aux ingrédients
      console.log('✅ Toutes les étapes créées avec succès!');
      this.createIngredientsAndLink(recetteId, ingredients);
      return;
    }

    const etape = etapes[index];
    console.log(`Envoi étape ${index + 1}/${etapes.length}:`, JSON.stringify(etape));

    this.recetteService.createEtape(etape).subscribe({
      next: (response: any) => {
        console.log(`✅ Étape ${index + 1} créée:`, response);
        // Créer l'étape suivante
        this.createEtapesSequentially(etapes, index + 1, recetteId, ingredients);
      },
      error: (error: any) => {
        this.loading = false;
        console.error(`❌ ERREUR lors de la création de l'étape ${index + 1}:`, error);
        
        let errorDetails = `Erreur lors de la création de l'étape ${index + 1}`;
        if (error.error && typeof error.error === 'string') {
          errorDetails = error.error;
        } else if (error.message) {
          errorDetails = error.message;
        }
        
        this.errorMessage = errorDetails;
        console.error('❌ IMPOSSIBLE de créer les étapes.');
      }
    });
  }

  private createIngredientsAndLink(recetteId: number, ingredients: any[]) {
    if (ingredients.length === 0) {
      // Pas d'ingrédients, recette terminée
      console.log('✅ Pas d\'ingrédients - recette terminée');
      this.finishRecetteCreation();
      return;
    }

    console.log('🔄 Création de', ingredients.length, 'ingrédient(s) de manière séquentielle...');
    this.createIngredientsSequentially(ingredients, 0, recetteId);
  }

  private createIngredientsSequentially(ingredients: any[], index: number, recetteId: number) {
    if (index >= ingredients.length) {
      // Tous les ingrédients sont traités
      console.log('✅ Tous les ingrédients créés et liés avec succès!');
      this.finishRecetteCreation();
      return;
    }

    const ingredient = ingredients[index];
    console.log(`Traitement ingrédient ${index + 1}/${ingredients.length}:`, ingredient.nom_ingredient);

    // Vérifier si l'ingrédient existe déjà
    const existingIngredient = this.allIngredients.find(
      (ing: any) => ing.nom_ingredient.toLowerCase() === ingredient.nom_ingredient.toLowerCase()
    );

    if (existingIngredient) {
      // L'ingrédient existe, créer la liaison directement
      console.log('✓ Ingrédient existant:', existingIngredient.nom_ingredient, '(ID:', existingIngredient.id + ')');
      
      this.recetteService.createRecetteIngredient({
        recetteId: recetteId,
        ingredientId: existingIngredient.id,
        quantite: ingredient.quantite
      }).subscribe({
        next: (response: any) => {
          console.log(`✅ Liaison créée pour l'ingrédient ${index + 1}`);
          // Continuer avec l'ingrédient suivant
          this.createIngredientsSequentially(ingredients, index + 1, recetteId);
        },
        error: (error: any) => {
          this.loading = false;
          console.error(`❌ Erreur lors de la liaison de l'ingrédient ${index + 1}:`, error);
          
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
      // Créer l'ingrédient d'abord, puis créer la liaison
      console.log('+ Création nouvel ingrédient:', ingredient.nom_ingredient);
      
      this.recetteService.createIngredient({ nom_ingredient: ingredient.nom_ingredient }).subscribe({
        next: (newIngredient: any) => {
          console.log('✅ Nouvel ingrédient créé:', newIngredient.nom_ingredient, '(ID:', newIngredient.id + ')');
          
          // Créer la liaison
          this.recetteService.createRecetteIngredient({
            recetteId: recetteId,
            ingredientId: newIngredient.id,
            quantite: ingredient.quantite
          }).subscribe({
            next: (linkResponse: any) => {
              console.log(`✅ Liaison créée pour l'ingrédient ${index + 1}`);
              // Continuer avec l'ingrédient suivant
              this.createIngredientsSequentially(ingredients, index + 1, recetteId);
            },
            error: (linkError: any) => {
              this.loading = false;
              console.error(`❌ Erreur lors de la liaison de l'ingrédient ${index + 1}:`, linkError);
              
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
          console.error(`❌ Erreur lors de la création de l'ingrédient ${index + 1}:`, error);
          
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
    console.log('🎉 Recette complètement créée et sauvegardée en DB');
    this.recetteForm.reset();
    this.etapesArray.clear();
    this.ingredientsArray.clear();
    this.imagePreview = null;
    this.selectedFile = null;

    setTimeout(() => {
      console.log('🔄 Redirection vers le dashboard...');
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
      // Vérifier que c'est une image
      if (!file.type.startsWith('image/')) {
        alert('Veuillez sélectionner une image valide');
        return;
      }

      // Vérifier la taille (max 5MB)
      const maxSize = 5 * 1024 * 1024; // 5MB
      if (file.size > maxSize) {
        alert('L\'image ne doit pas dépasser 5MB');
        return;
      }

      this.selectedFile = file;

      // Créer une prévisualisation
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.imagePreview = e.target.result;
        // Mettre à jour le formulaire avec le nom du fichier pour l'instant
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
}}