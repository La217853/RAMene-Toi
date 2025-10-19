namespace RameneToi.Models
{
    public class Adresse
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int Numero { get; set; }
        public string Rue { get; set; } = null!;

        //Relation en Adresse et Utilisateur
        //Une adresse peut être associée à plusieurs utilisateurs
        //Un utilisateur ne peut avoir qu'une seule adresse
        public List<Utilisateurs> Utilisateur { get; set; } = new();
    }
}
