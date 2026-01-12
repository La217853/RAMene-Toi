import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Recette } from '../../Models/recette.model';

@Component({
  selector: 'app-recipe-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './recipe-card.component.html',
  styleUrls: ['./recipe-card.component.css']
})
export class RecipeCardComponent {
  @Input({ required: true }) recette!: Recette;
  private router = inject(Router);

  navigateToRecette() {
    this.router.navigate(['/recette', this.recette.id]);
  }
}
