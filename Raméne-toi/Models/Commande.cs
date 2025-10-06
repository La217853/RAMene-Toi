namespace Raméne_toi.Models;

public class Commande
{
    public int IdCommande { get; set; }
    public int IdUtilisateur { get; set; }
    public int IdConfigPc { get; set; }
    public double Prix { get; set; }
}