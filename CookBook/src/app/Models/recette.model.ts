export interface Recette {
  id: number;
  titre: string;
  description?: string;
  tempsPreparation?: number;
  tempsCuisson?: number;
  difficulte?: string;
  portions?: number;
  categorieId?: number;
  utilisateurId: number;
  dateCreation?: Date;
}
