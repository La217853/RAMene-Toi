import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IngredientService } from '../ingredient-service';
import { Ingredient } from '../models/ingredient.models';

@Component({
  selector: 'app-ingredient',
  standalone: true, 
  imports: [CommonModule],
  templateUrl: './ingredient.component.html',
  styleUrls: ['./ingredient.component.css']
})
export class IngredientComponent implements OnInit {

  ingredients: Ingredient[] = [];

  constructor(private ingredientService: IngredientService) {}

  ngOnInit(): void {
    this.loadIngredients();
  }

  loadIngredients(): void {
    this.ingredientService.getIngredients().subscribe(data => {
      this.ingredients = data;
    });
  }

  addIngredient(): void {
    const newIngredient: Ingredient = {
      name: 'Nouvel ingrédient'
    };
    this.ingredientService.addIngredient(newIngredient).subscribe(addedIngredient => {
      this.ingredients.push(addedIngredient);
    });
  }
}
