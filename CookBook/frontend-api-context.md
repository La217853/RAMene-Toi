# CookBook API - Documentation pour Frontend

Ce document décrit l'API backend du projet CookBook pour faciliter le développement frontend.

## Configuration de Base

- **URL de base (développement)**: `https://localhost:[port]/api`
- **Authentication**: JWT Bearer Token
- **Format**: JSON

## Authentification

### Obtenir un Token JWT
Les endpoints nécessitent un header `Authorization: Bearer <token>` sauf indication contraire.

Pour obtenir un token, utilisez l'endpoint d'authentification (à implémenter ou demander au backend).

---

## Endpoints API

### 1. **Catégories** (`/api/Categories`)

#### GET `/api/Categories`
- **Auth**: Requise
- **Description**: Récupère toutes les catégories
- **Response**: `Array<Categorie>`
```json
[
  {
    "id": 1,
    "nom_categorie": "Desserts"
  }
]
```

#### GET `/api/Categories/{id}`
- **Auth**: Requise
- **Description**: Récupère une catégorie par ID
- **Response**: `Categorie`

#### POST `/api/Categories`
- **Auth**: Requise (Admin seulement)
- **Description**: Crée une nouvelle catégorie
- **Body**:
```json
{
  "nom_categorie": "Nouvelle Catégorie"
}
```

#### PUT `/api/Categories/{id}`
- **Auth**: Requise (Admin seulement)
- **Description**: Met à jour une catégorie
- **Body**: `Categorie` complet

#### DELETE `/api/Categories/{id}`
- **Auth**: Requise (Admin seulement)
- **Description**: Supprime une catégorie

---

### 2. **Recettes** (`/api/Recettes`)

#### GET `/api/Recettes`
- **Auth**: Requise
- **Description**: Récupère toutes les recettes
- **Response**: `Array<Recette>`
```json
[
  {
    "id": 1,
    "titre_recette": "Gâteau au chocolat",
    "description_recette": "Un délicieux gâteau...",
    "photo_recette": "url/to/photo.jpg",
    "utilisateurId": 1,
    "categorieId": 2
  }
]
```

#### GET `/api/Recettes/{id}`
- **Auth**: Requise
- **Description**: Récupère une recette par ID
- **Response**: `Recette`

#### GET `/api/Recettes/Categorie/{categorieId}`
- **Auth**: Requise
- **Description**: Récupère toutes les recettes d'une catégorie
- **Response**: `Array<Recette>`

#### GET `/api/Recettes/Utilisateur/{utilisateurId}`
- **Auth**: Requise
- **Description**: Récupère toutes les recettes créées par un utilisateur
- **Response**: `Array<Recette>`

#### POST `/api/Recettes`
- **Auth**: Requise
- **Description**: Crée une nouvelle recette
- **Body**:
```json
{
  "titre_recette": "Ma recette",
  "description_recette": "Description...",
  "photo_recette": "url/to/photo.jpg",
  "utilisateurId": 1,
"categorieId": 2
}
```

#### PUT `/api/Recettes/{id}`
- **Auth**: Requise
- **Description**: Met à jour une recette
- **Body**: `Recette` complet

#### DELETE `/api/Recettes/{id}`
- **Auth**: Requise
- **Description**: Supprime une recette

---

### 3. **Utilisateurs** (`/api/Utilisateurs`)

#### GET `/api/Utilisateurs`
- **Auth**: Requise
- **Description**: Récupère tous les utilisateurs avec leur rôle
- **Response**: `Array<Utilisateur>`
```json
[
  {
    "id": 1,
    "pseudo": "JohnDoe",
    "email": "john@example.com",
"motDePasse": "hashed_password",
    "roleId": 1,
    "role": {
      "id": 1,
      "nom_role": "Admin"
    }
  }
]
```

#### GET `/api/Utilisateurs/{id}`
- **Auth**: Requise
- **Description**: Récupère un utilisateur par ID avec son rôle
- **Response**: `Utilisateur`

#### GET `/api/Utilisateurs/{id}/favoris`
- **Auth**: Requise
- **Description**: Récupère les recettes favorites d'un utilisateur
- **Response**: `Array<Recette>`

#### POST `/api/Utilisateurs`
- **Auth**: Non requise (AllowAnonymous)
- **Description**: Crée un nouvel utilisateur (inscription)
- **Body**:
```json
{
  "pseudo": "NewUser",
  "email": "user@example.com",
  "motDePasse": "password123",
  "roleId": 2
}
```

#### PUT `/api/Utilisateurs/{id}`
- **Auth**: Non requise (AllowAnonymous)
- **Description**: Met à jour un utilisateur
- **Body**: `Utilisateur` complet

#### DELETE `/api/Utilisateurs/{id}`
- **Auth**: Requise
- **Description**: Supprime un utilisateur

---

### 4. **Ingrédients** (`/api/Ingredients`)

#### GET `/api/Ingredients`
- **Auth**: Requise
- **Description**: Récupère tous les ingrédients
- **Response**: `Array<Ingredient>`
```json
[
  {
    "id": 1,
    "nom_ingredient": "Farine"
  }
]
```

#### GET `/api/Ingredients/{id}`
- **Auth**: Requise
- **Description**: Récupère un ingrédient par ID
- **Response**: `Ingredient`

#### POST `/api/Ingredients`
- **Auth**: Requise
- **Description**: Crée un nouvel ingrédient
- **Body**:
```json
{
  "nom_ingredient": "Sucre"
}
```

#### PUT `/api/Ingredients/{id}`
- **Auth**: Requise
- **Description**: Met à jour un ingrédient
- **Body**: `Ingredient` complet

#### DELETE `/api/Ingredients/{id}`
- **Auth**: Requise
- **Description**: Supprime un ingrédient

---

## Modèles de Données

### Categorie
```typescript
interface Categorie {
  id: number;
  nom_categorie: string;
}
```

### Recette
```typescript
interface Recette {
  id: number;
  titre_recette: string;
  description_recette: string;
  photo_recette: string;
  utilisateurId: number;
  categorieId: number;
}
```

### Utilisateur
```typescript
interface Utilisateur {
  id: number;
  pseudo: string;
  email: string;
  motDePasse: string;
  roleId: number;
  role?: Role;
}
```

### Role
```typescript
interface Role {
  id: number;
  nom_role: string; // "Admin" ou "Utilisateur"
}
```

### Ingredient
```typescript
interface Ingredient {
  id: number;
  nom_ingredient: string;
}
```

### Etapes
```typescript
interface Etapes {
  id: number;
  titre_etape: number;
  description_etape: string;
  id_recette: number;
}
```

### RecetteIngredient
```typescript
interface RecetteIngredient {
  recetteId: number;
  ingredientId: number;
  quantite: string; // ex: "2 tasses", "1 cuillère à soupe"
}
```

---

## Codes de Statut HTTP

- **200 OK**: Requête réussie
- **201 Created**: Ressource créée avec succès
- **204 No Content**: Mise à jour/suppression réussie
- **400 Bad Request**: Données invalides
- **401 Unauthorized**: Token manquant ou invalide
- **403 Forbidden**: Accès refusé (rôle insuffisant)
- **404 Not Found**: Ressource non trouvée
- **500 Internal Server Error**: Erreur serveur

---

## Gestion des Erreurs

Les erreurs retournent généralement un objet avec des détails:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "errors": {
"field": ["Error message"]
  }
}
```

---

## Notes pour le Frontend

1. **Rôles**: Il existe deux rôles - "Admin" (gestion complète) et "Utilisateur" (accès limité)
2. **Relations**: 
   - Une recette appartient à un utilisateur et une catégorie
   - Un utilisateur peut avoir plusieurs recettes favorites (relation many-to-many)
   - Une recette contient plusieurs étapes et ingrédients
3. **Swagger**: L'API expose une documentation Swagger en développement à `/swagger`
4. **CORS**: Assurez-vous de configurer CORS si votre frontend est sur un domaine différent

---

## Exemple d'utilisation (JavaScript/Fetch)

```javascript
// Récupérer toutes les recettes
const token = 'votre_jwt_token';

fetch('https://localhost:7000/api/Recettes', {
  method: 'GET',
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  }
})
.then(response => response.json())
.then(data => console.log(data))
.catch(error => console.error('Error:', error));

// Créer une nouvelle recette
fetch('https://localhost:7000/api/Recettes', {
  method: 'POST',
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({
    titre_recette: "Ma nouvelle recette",
    description_recette: "Description...",
    photo_recette: "photo.jpg",
    utilisateurId: 1,
    categorieId: 2
  })
})
.then(response => response.json())
.then(data => console.log(data));
```

---

## Exemple d'utilisation (React/Axios)

```javascript
import axios from 'axios';

const API_BASE_URL = 'https://localhost:7000/api';
const token = localStorage.getItem('token');

// Configuration d'axios avec le token
const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  }
});

// Récupérer toutes les recettes
const getRecettes = async () => {
  try {
    const response = await api.get('/Recettes');
    return response.data;
  } catch (error) {
    console.error('Erreur:', error);
    throw error;
  }
};

// Récupérer les recettes d'une catégorie
const getRecettesByCategorie = async (categorieId) => {
  try {
    const response = await api.get(`/Recettes/Categorie/${categorieId}`);
    return response.data;
  } catch (error) {
    console.error('Erreur:', error);
    throw error;
  }
};

// Créer une recette
const createRecette = async (recetteData) => {
  try {
    const response = await api.post('/Recettes', recetteData);
    return response.data;
  } catch (error) {
    console.error('Erreur:', error);
    throw error;
  }
};

// Ajouter une recette aux favoris
const addToFavoris = async (utilisateurId, recetteId) => {
  try {
    const response = await api.post(`/Utilisateurs/${utilisateurId}/favoris/${recetteId}`);
    return response.data;
  } catch (error) {
    console.error('Erreur:', error);
    throw error;
  }
};
```

---

## Points d'attention pour l'implémentation Frontend

### Authentification
- Stocker le token JWT dans le localStorage ou sessionStorage
- Ajouter le token dans chaque requête via le header Authorization
- Gérer l'expiration du token et rediriger vers la page de connexion si nécessaire
- Implémenter un refresh token si applicable

### Gestion des Rôles
- Afficher/masquer les fonctionnalités selon le rôle (Admin vs Utilisateur)
- Les actions de création/modification/suppression des catégories sont réservées aux Admin
- Tous les utilisateurs authentifiés peuvent gérer les recettes et ingrédients

### Validations côté client
- Email: format valide
- Mot de passe: longueur minimale (à définir selon vos règles)
- Champs obligatoires: pseudo, email, titre de recette, etc.

### UX Recommandations
- Afficher des messages de succès/erreur après chaque action
- Implémenter un loader pendant les requêtes
- Gérer les cas où les listes sont vides
- Pagination recommandée pour les grandes listes de recettes
