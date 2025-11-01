using Microsoft.EntityFrameworkCore;
using RameneToi.Data;
using RameneToi.Models;

namespace RameneToi.Tests.Fixture
{
    public class DatabaseFixture : IDisposable
    {
   public RameneToiWebAPIContext CreateContext()
      {
          // Créer une nouvelle base de données InMemory pour chaque test
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
 // Données de test de base
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
     Adresse = adresse
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
 // Rien à faire ici puisque chaque contexte est créé et disposé par test
}
    }
}
