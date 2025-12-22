import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { IngredientService } from '../ingredient-service';
import { Ingredient } from '../Models/ingredient.models'; 

@Component({
  selector: 'app-ingredient-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './ingredient-form.component.html', 
  styleUrls: ['./ingredient-form.component.css']   
})
export class IngredientFormComponent {
  ingredient: Ingredient = { name: ''};

  constructor(private ingredientService: IngredientService) {}

  onSubmit(): void {
    this.ingredientService.addIngredient(this.ingredient).subscribe({
      next: (newIngredient) => {
        console.log('Ingrédient ajouté :', newIngredient);
        this.ingredient = { name: '' }; // reset complet
      },
      error: (err) => {
        console.error('Erreur lors de l’ajout :', err);
      }
    });
  }
}
