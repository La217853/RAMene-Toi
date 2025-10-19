namespace RameneToi.Models
{
    public class Composant
    {
        public int Id { get; set; }
        public string Nom { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Marque { get; set; } = null!;
        public string Modele { get; set; } = null!;
        public string Prix { get; set; } = null!;
        public string Stock { get; set; } = null!;
        public string Score { get; set; } = null!;

        //Un Composant peut être dans plusieurs configurations
        //Une configuration peut contenir plusieurs composants
        public List<ConfigurationPc> Configurations { get; set; } = new();
    }
}
