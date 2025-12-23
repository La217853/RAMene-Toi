import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RecetteService } from '../../Services/recette.service';
import { Recette } from '../../Models/recette.model';
import { CommonModule } from '@angular/common';
import { forkJoin } from 'rxjs';
import { IngredientQuantite } from '../../Models/ingredient.models';


@Component({
  selector: 'app-recette-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './details-recette.html',
  styleUrls: ['./details-recette.css']
})
export class RecetteDetailsComponent implements OnInit {

  recette?: Recette;
  ingredients: IngredientQuantite[] = [];
  loading = true;
  error = false;
  hasImage = false;

  constructor(
    private route: ActivatedRoute,
    private recetteService: RecetteService
  ) {}

  ngOnInit(): void {
  const id = Number(this.route.snapshot.paramMap.get('id'));

  forkJoin({
    recette: this.recetteService.getRecetteById(id),
    ingredients: this.recetteService.getRecetteIngredients(id)
  }).subscribe({
    next: async (data) => {
  this.recette = data.recette;
  this.ingredients = data.ingredients;

  const photo = this.recette?.photo_recette?.trim();

  if (!photo) {
    this.recette.photo_recette = undefined;
  }
  else if (photo.startsWith('http')) {
    this.recette.photo_recette = photo;
  }
  else {
    this.recette.photo_recette = `assets/images/${photo}`;
  }


  for (let ing of this.ingredients) {
    ing.detail = await this.recetteService.getIngredient(ing.ingredientId).toPromise();
  }

  this.loading = false;
},
    error: () => {
      this.error = true;
      this.loading = false;
    }
  });
}

onImageError() { 
  this.hasImage = false;
} 


}
