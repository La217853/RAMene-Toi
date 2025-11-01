using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RameneToi.Models
{
    public class Utilisateurs
    {
        public int Id { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }

        [Required]
        public string MotDePasse { get; set; }

        //Foreign key vers l'entité Adresse 
        public int AdresseId { get; set; }
        [JsonIgnore]
        public Adresse? Adresse { get; set; } = null!;

        [JsonIgnore]
        [ValidateNever]
        public List<ConfigurationPc> ConfigurationsPc { get; set; }

        // Propriété de navigation pour la relation un-à-plusieurs (avec Commande)
        [JsonIgnore]
        [ValidateNever]
        public List<Commande> Commandes { get; set; }

        
        [JsonPropertyName("CommandeId")] //Propriete de nom qui sera dans le Json
        //Rajouter l'attribut CommandeId dans le json de l'utilisateur et remplir avec les Id des commandes associés
        public List<int> CommandeId => Commandes?.Select(u => u.Id).ToList() ?? new List<int>();
    }
}
