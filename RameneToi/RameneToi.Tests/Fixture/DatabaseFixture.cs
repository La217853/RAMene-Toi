using Microsoft.EntityFrameworkCore;
using RameneToi.Data;
using RameneToi.Models;

namespace RameneToi.Tests.Fixture
{
    public class DatabaseFixture : IDisposable
    {
   public RameneToiWebAPIContext CreateContext()
      {
          // Cr�er une nouvelle base de donn�es InMemory pour chaque test
   var options = new DbContextOptionsBuilder<RameneToiWebAPIContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;

   var context = new RameneToiWebAPIContext(options);
            context.Database.EnsureCreated();
   SeedData(context);
  return context;
        }

private void SeedData(RameneToiWebAPIContext context)
     {
 // Donn�es de test de base
 var adresse = new Adresse
  {
Id = 1,
     Code = 75001,
 Numero = 10,
    Rue = "Rue de Test"
   };

var utilisateur = new Utilisateurs
      {
          Id = 1,
       Prenom = "Jean",
     Nom = "Dupont",
     Email = "jean.dupont@test.com",
     MotDePasse = "hashedPassword123",
     AdresseId = 1,
     Adresse = adresse,
    Roles = new List<string> { "admin" }
};

       var composant = new Composant
       {
     Id = 1,
Type = "CPU",
    Marque = "Intel",
         Modele = "i9-13900K",
    Prix = 699.99f,
       Stock = 10,
    Score = 95
};

       context.Adresses.Add(adresse);
       context.Utilisateurs.Add(utilisateur);
          context.Composants.Add(composant);
   context.SaveChanges();
        }

 public void Dispose()
   {
 // Rien � faire ici puisque chaque contexte est cr�� et dispos� par test
}
    }
}
