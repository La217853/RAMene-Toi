export interface Ingredient {
  id?: number;
  nom_ingredient: string;
}

export interface IngredientQuantite {
  recetteId: number;
  ingredientId: number;
  quantite: string;
  detail?: Ingredient; 
}
