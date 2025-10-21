using System.Text.Json.Serialization;

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
        public int? AdresseId { get; set; }
        [JsonIgnore]
        public Adresse? Adresse { get; set; } = null!;

        //new() = juste initialisation de la liste automatiquement
        public List<ConfigurationPc> Configurations { get; set; } = new();
        public List<Commande> Commandes { get; set; } = new();
    }
}
