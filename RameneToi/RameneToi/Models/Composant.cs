using System.Text.Json.Serialization;

namespace RameneToi.Models
{
    public class Composant
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string Marque { get; set; } = null!;
        public string Modele { get; set; } = null!;
        public int Prix { get; set; }
        public int Stock { get; set; }
        public int Score { get; set; }

        //Un Composant peut être dans plusieurs configurations
        //Une configuration peut contenir plusieurs composants
        [JsonIgnore]
        public List<ConfigurationPc>? Configurations { get; set; }
    }
}
