using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RameneToi.Models
{
    public class Adresse
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int Numero { get; set; }
        public string Rue { get; set; }

        [JsonIgnore]
        public List<Utilisateurs>? utilisateur { get; set; }

       
        [JsonPropertyName("utilisateurId")] //Propriete de nom qui sera dans le Json
        //Rajouter l'attribut UtilisateurId dans le json de l'adresse et remplir avec les Id des utilisateurs associés
        //Si null, renvoyer une liste vide
        public List<int> UtilisateurId => utilisateur?.Select(u => u.Id).ToList() ?? new List<int>();

    }
}
