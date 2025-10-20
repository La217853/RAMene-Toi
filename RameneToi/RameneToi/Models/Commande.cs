using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RameneToi.Models
{
    public class Commande
    {
        public int Id { get; set; }

        // 1 Utilisateur → N Commandes
        public int UtilisateurId { get; set; }
        [JsonIgnore]
        public Utilisateurs Utilisateur { get; set; } = null!;

        // 1–1 avec ConfigurationPc
        public int ConfigurationPcId { get; set; }
        
        public ConfigurationPc ?ConfigurationPc { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrixConfiguration { get; set; }
    }
}
