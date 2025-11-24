export interface Role {
  id: number;
  nom_role: string;
}

export interface User {
  id: number;
  pseudo: string;
  email: string;
  roleId: number;
  role?: Role;
}