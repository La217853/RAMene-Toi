using Xunit;
using RameneToi.Models;
using RameneToi.Tests.Fixture;
using Microsoft.EntityFrameworkCore;

namespace RameneToi.Tests.Integration.Repositories
{
    public class ConfigurationPcRepositoryTests : IClassFixture<DatabaseFixture>
{
        private readonly DatabaseFixture _fixture;

        public ConfigurationPcRepositoryTests(DatabaseFixture fixture)
 {
          _fixture = fixture;
   }

     [Fact]
        public async Task CreateConfigurationPc_ShouldSaveToDatabase()
  {
         // Arrange
  using var context = _fixture.CreateContext();
       var config = new ConfigurationPc
            {
  NomConfiguration = "Workstation Pro",
      UtilisateurId = 1
      };

      // Act
      context.ConfigurationPcs.Add(config);
   await context.SaveChangesAsync();

       // Assert
            var saved = await context.ConfigurationPcs
    .FirstOrDefaultAsync(c => c.NomConfiguration == "Workstation Pro");

      Assert.NotNull(saved);
     Assert.Equal(1, saved.UtilisateurId);
        }

   [Fact]
      public async Task GetConfigurationPcWithComposants_ShouldIncludeComposants()
   {
       // Arrange
    using var context = _fixture.CreateContext();
var composant1 = new Composant
      {
    Type = "GPU",
        Marque = "AMD",
     Modele = "RX 7900 XTX",
          Prix = 999.99f,
   Stock = 5,
   Score = 90
        };

    var composant2 = new Composant
     {
          Type = "CPU",
      Marque = "AMD",
      Modele = "Ryzen 9 7950X",
   Prix = 699.99f,
  Stock = 10,
    Score = 95
     };

  var config = new ConfigurationPc
  {
      NomConfiguration = "AMD Build",
     UtilisateurId = 1,
  Composants = new List<Composant> { composant1, composant2 }
   };

       context.ConfigurationPcs.Add(config);
       context.Composants.AddRange(composant1, composant2);
          await context.SaveChangesAsync();

       // Act
var result = await context.ConfigurationPcs
    .Include(c => c.Composants)
 .FirstOrDefaultAsync(c => c.Id == config.Id);

  // Assert
 Assert.NotNull(result);
            Assert.NotNull(result.Composants);
            Assert.Equal(2, result.Composants.Count);
            Assert.Contains(result.Composants, c => c.Type == "GPU");
       Assert.Contains(result.Composants, c => c.Type == "CPU");
        }

        [Fact]
   public async Task GetConfigurationPcWithUtilisateur_ShouldIncludeUser()
        {
     // Arrange
         using var context = _fixture.CreateContext();
       var config = new ConfigurationPc
      {
    NomConfiguration = "Test Config User",
         UtilisateurId = 1
   };

   context.ConfigurationPcs.Add(config);
          await context.SaveChangesAsync();

    // Act
       var result = await context.ConfigurationPcs
     .Include(c => c.Utilisateur)
        .FirstOrDefaultAsync(c => c.Id == config.Id);

        // Assert
        Assert.NotNull(result);
            Assert.NotNull(result.Utilisateur);
  Assert.Equal("Jean", result.Utilisateur.Prenom);
     Assert.Equal("Dupont", result.Utilisateur.Nom);
        }

   [Fact]
     public async Task UpdateConfigurationPc_ShouldModifyConfiguration()
        {
   // Arrange
      using var context = _fixture.CreateContext();
 var config = new ConfigurationPc
     {
      NomConfiguration = "Old Name",
       UtilisateurId = 1
  };

   context.ConfigurationPcs.Add(config);
        await context.SaveChangesAsync();

    // Act
   config.NomConfiguration = "New Name";
            context.ConfigurationPcs.Update(config);
  await context.SaveChangesAsync();

            // Assert
 var updated = await context.ConfigurationPcs.FindAsync(config.Id);
Assert.NotNull(updated);
      Assert.Equal("New Name", updated.NomConfiguration);
        }

   [Fact]
   public async Task DeleteConfigurationPc_ShouldRemoveFromDatabase()
        {
     // Arrange
      using var context = _fixture.CreateContext();
     var config = new ConfigurationPc
  {
    NomConfiguration = "To Be Deleted",
      UtilisateurId = 1
      };

      context.ConfigurationPcs.Add(config);
 await context.SaveChangesAsync();
    var configId = config.Id;

    // Act
   context.ConfigurationPcs.Remove(config);
     await context.SaveChangesAsync();

  // Assert
 var deleted = await context.ConfigurationPcs.FindAsync(configId);
    Assert.Null(deleted);
        }

     [Fact]
      public async Task AddComposantToConfiguration_ShouldUpdateRelation()
   {
   // Arrange
    using var context = _fixture.CreateContext();
var config = new ConfigurationPc
      {
         NomConfiguration = "Base Config",
    UtilisateurId = 1,
       Composants = new List<Composant>()
   };

      var composant = new Composant
      {
       Type = "RAM",
    Marque = "Corsair",
      Modele = "Vengeance 32GB",
   Prix = 149.99f,
      Stock = 20,
     Score = 85
    };

     context.ConfigurationPcs.Add(config);
            context.Composants.Add(composant);
await context.SaveChangesAsync();

    // Act
            config.Composants.Add(composant);
      await context.SaveChangesAsync();

  // Assert
 var updated = await context.ConfigurationPcs
       .Include(c => c.Composants)
   .FirstOrDefaultAsync(c => c.Id == config.Id);

  Assert.NotNull(updated);
       Assert.Single(updated.Composants);
 Assert.Equal("RAM", updated.Composants.First().Type);
        }

      [Fact]
        public async Task ConfigurationPc_WithCommande_ShouldHaveOneToOneRelation()
        {
      // Arrange
using var context = _fixture.CreateContext();
var config = new ConfigurationPc
    {
   NomConfiguration = "Config with Commande",
     UtilisateurId = 1
     };

    context.ConfigurationPcs.Add(config);
await context.SaveChangesAsync();

       var commande = new Commande
   {
    UtilisateurId = 1,
      ConfigurationPcId = config.Id,
     PrixConfiguration = 1500m
            };

   context.Commandes.Add(commande);
            await context.SaveChangesAsync();

     // Act
  var result = await context.ConfigurationPcs
    .Include(c => c.Commande)
.FirstOrDefaultAsync(c => c.Id == config.Id);

       // Assert
       Assert.NotNull(result);
 Assert.NotNull(result.Commande);
     Assert.Equal(1500m, result.Commande.PrixConfiguration);
        }
  }
}
