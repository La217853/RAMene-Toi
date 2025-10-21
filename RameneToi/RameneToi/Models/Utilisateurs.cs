namespace RameneToi.Models
{
    public class Utilisateurs
    {
        public int Id { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }

        //Foreign key vers l'entité Adresse 
        public int AdresseId { get; set; }
        public Adresse Adresse { get; set; } = null!;

        public List<ConfigurationPc> ConfigurationsPc { get; set; }

        // Propriété de navigation pour la relation un-à-plusieurs (avec Commande)
        public List<Commande> Commandes { get; set; }
    }
}
