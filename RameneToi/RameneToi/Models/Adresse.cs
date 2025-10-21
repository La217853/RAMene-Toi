namespace RameneToi.Models
{
    public class Adresse
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int Numero { get; set; }
        public string Rue { get; set; }

        public Utilisateurs utilisateur { get; set; }
    }
}
