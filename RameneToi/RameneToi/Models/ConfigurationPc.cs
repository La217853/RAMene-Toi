namespace RameneToi.Models
{
    public class ConfigurationPc
    {
        public int Id { get; set; }
        public string NomConfiguration { get; set; } = null!;

        // 1 Utilisateur → N Configurations
        public int UtilisateurId { get; set; }
        public Utilisateurs Utilisateur { get; set; } = null!;

        public List<Composant> Composants { get; set; } = new();


        //Relation 1-1, une Commande est reliée à une seule ConfigurationPc
        //Mais une configurationPc ne peut avoir que une seule Commande
        // 1 Config = 0 ou 1 Commande
        public Commande? Commande { get; set; }
    }
}
