namespace Raméne_toi.Models;

public class Commande
{
    public int IdCommande { get; set; }
    public int Idutilisateur { get; set; }
    public int IdConfigPc { get; set; }
    public double Prix { get; set; }
}