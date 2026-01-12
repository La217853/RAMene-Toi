export interface Recette {
  id: number;
  titre_recette: string;
  description_recette?: string;
  photo_recette?: string;
  utilisateurId: number;
  categorieId?: number;
}



export interface Etape {
  id: number;
  titre_etape: string;
  description_etape: string;
}

export interface Categorie {
  id: number;
  nom: string;
}


