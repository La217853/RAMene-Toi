import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecetteService } from '../../Services/recette.service';
import { Recette,Categorie } from '../../Models/recette.model';
import { Router } from '@angular/router';
import { RecipeCardComponent } from '../../Shared/recipe-card/recipe-card.component';



@Component({
  selector: 'app-decouvrir',
  standalone: true,
  imports: [CommonModule,RecipeCardComponent],
  templateUrl: './decouvrir.html',
  styleUrls: ['./decouvrir.css']
})
export class DecouvrirComponent implements OnInit {

  recettes: Recette[] = [];
  filteredRecettes:Recette[] = [];
  searchTerm = '';
  categories: Categorie[] = [];
  selectedCategorie: number | null = null;
  loading = true;
  error = false;
  
  constructor(
    private recetteService: RecetteService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.recetteService.getAllRecettes().subscribe({
      next: (data) => {
        this.recettes = data;
        this.filteredRecettes = data;
        this.loading = false;
      },
      error: () => {
        this.error = true;
        this.loading = false;
      }
    });

    this.recetteService.getCategories().subscribe({ 
      next: (cats) => { 
        this.categories = cats; 
      } 
    });
  }

  openRecette(id: number) {
    this.router.navigate(['/recette', id]);
  }

  applyFilters() {
  this.filteredRecettes = this.recettes.filter(r => {

    const matchSearch =
      this.searchTerm.trim().length === 0 ||
      r.titre_recette.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      r.description_recette?.toLowerCase().includes(this.searchTerm.toLowerCase());

    const matchCategorie =
      this.selectedCategorie === null ||
      r.categorieId === this.selectedCategorie;

    return matchSearch && matchCategorie;
  });
}

onSearchChange(value: string) {
  this.searchTerm = value;
  this.applyFilters();
}

onCategorieChange(value: string) {
  this.selectedCategorie = value === '' ? null : Number(value);
  this.applyFilters();
}


}
