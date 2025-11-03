using Xunit;
using RameneToi.Models;
using RameneToi.Tests.Fixture;
using Microsoft.EntityFrameworkCore;

namespace RameneToi.Tests.Integration.Repositories
{
    public class UtilisateursRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public UtilisateursRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
    }

        [Fact]
     public async Task CreateUtilisateur_ShouldSaveToDatabase()
        {
          // Arrange
            using var context = _fixture.CreateContext();
         var adresse = new Adresse
     {
      Code = 69001,
      Numero = 25,
         Rue = "Rue Victor Hugo"
            };
      context.Adresses.Add(adresse);
          await context.SaveChangesAsync();

      var utilisateur = new Utilisateurs
        {
         Prenom = "Marie",
    Nom = "Curie",
                Email = "marie.curie@test.com",
       MotDePasse = "password123",
       AdresseId = adresse.Id
            };

            // Act
  context.Utilisateurs.Add(utilisateur);
            await context.SaveChangesAsync();

    // Assert
      var saved = await context.Utilisateurs
     .FirstOrDefaultAsync(u => u.Email == "marie.curie@test.com");

            Assert.NotNull(saved);
            Assert.Equal("Marie", saved.Prenom);
          Assert.Equal("Curie", saved.Nom);
 }

        [Fact]
        public async Task GetUtilisateurWithAdresse_ShouldIncludeNavigationProperty()
   {
  // Arrange
            using var context = _fixture.CreateContext();
   var existingUser = await context.Utilisateurs
 .Include(u => u.Adresse)
    .FirstOrDefaultAsync(u => u.Id == 1);

  // Assert
    Assert.NotNull(existingUser);
            Assert.NotNull(existingUser.Adresse);
            Assert.Equal("Rue de Test", existingUser.Adresse.Rue);
      Assert.Equal(75001, existingUser.Adresse.Code);
      }

    [Fact]
        public async Task GetUtilisateurWithConfigurationsPc_ShouldIncludeConfigurations()
        {
            // Arrange
  using var context = _fixture.CreateContext();
            var config1 = new ConfigurationPc
         {
     NomConfiguration = "Gaming PC",
                UtilisateurId = 1
  };
            var config2 = new ConfigurationPc
            {
            NomConfiguration = "Work PC",
    UtilisateurId = 1
    };

      context.ConfigurationPcs.AddRange(config1, config2);
    await context.SaveChangesAsync();

   // Act
     var utilisateur = await context.Utilisateurs
     .Include(u => u.ConfigurationsPc)
          .FirstOrDefaultAsync(u => u.Id == 1);

            // Assert
  Assert.NotNull(utilisateur);
         Assert.NotNull(utilisateur.ConfigurationsPc);
    Assert.Contains(utilisateur.ConfigurationsPc, c => c.NomConfiguration == "Gaming PC");
 Assert.Contains(utilisateur.ConfigurationsPc, c => c.NomConfiguration == "Work PC");
      }

        [Fact]
        public async Task UpdateUtilisateur_ShouldModifyExistingUser()
   {
       // Arrange
            using var context = _fixture.CreateContext();
   var adresse = new Adresse { Code = 33000, Numero = 5, Rue = "Rue Nouvelle" };
 context.Adresses.Add(adresse);
     await context.SaveChangesAsync();

   var utilisateur = new Utilisateurs
    {
       Prenom = "Test",
     Nom = "User",
             Email = "test.user@example.com",
     MotDePasse = "pass",
           AdresseId = adresse.Id
            };
    context.Utilisateurs.Add(utilisateur);
  await context.SaveChangesAsync();

            // Act
     utilisateur.Prenom = "Updated";
       utilisateur.Nom = "Name";
            context.Utilisateurs.Update(utilisateur);
            await context.SaveChangesAsync();

     // Assert
     var updated = await context.Utilisateurs.FindAsync(utilisateur.Id);
      Assert.NotNull(updated);
      Assert.Equal("Updated", updated.Prenom);
            Assert.Equal("Name", updated.Nom);
        }

     [Fact]
        public async Task DeleteUtilisateur_ShouldRemoveFromDatabase()
        {
        // Arrange
      using var context = _fixture.CreateContext();
            var adresse = new Adresse { Code = 44000, Numero = 10, Rue = "Rue à supprimer" };
 context.Adresses.Add(adresse);
      await context.SaveChangesAsync();

       var utilisateur = new Utilisateurs
     {
       Prenom = "ToDelete",
          Nom = "User",
          Email = "delete@test.com",
        MotDePasse = "pass",
   AdresseId = adresse.Id
        };
            context.Utilisateurs.Add(utilisateur);
    await context.SaveChangesAsync();
         var userId = utilisateur.Id;

     // Act
          context.Utilisateurs.Remove(utilisateur);
            await context.SaveChangesAsync();

        // Assert
            var deleted = await context.Utilisateurs.FindAsync(userId);
          Assert.Null(deleted);
  }

[Fact]
        public async Task FindUtilisateurByEmail_ShouldReturnCorrectUser()
        {
        // Arrange
            using var context = _fixture.CreateContext();
  var adresse = new Adresse { Code = 59000, Numero = 15, Rue = "Rue Email Test" };
 context.Adresses.Add(adresse);
     await context.SaveChangesAsync();

            var utilisateur = new Utilisateurs
{
         Prenom = "Find",
     Nom = "ByEmail",
      Email = "unique.email@test.com",
       MotDePasse = "pass",
                AdresseId = adresse.Id
            };
            context.Utilisateurs.Add(utilisateur);
            await context.SaveChangesAsync();

            // Act
            var found = await context.Utilisateurs
 .FirstOrDefaultAsync(u => u.Email == "unique.email@test.com");

            // Assert
         Assert.NotNull(found);
            Assert.Equal("Find", found.Prenom);
      Assert.Equal("ByEmail", found.Nom);
        }
    }
}
