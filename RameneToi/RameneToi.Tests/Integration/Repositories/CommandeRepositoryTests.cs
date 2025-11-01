using Xunit;
using RameneToi.Models;
using RameneToi.Tests.Fixture;
using Microsoft.EntityFrameworkCore;

namespace RameneToi.Tests.Integration.Repositories
{
    public class CommandeRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

     public CommandeRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
     public async Task CreateCommande_ShouldSaveToDatabase()
    {
          // Arrange
    using var context = _fixture.CreateContext();
        var config = new ConfigurationPc
            {
           NomConfiguration = "PC Test",
  UtilisateurId = 1
 };
        context.ConfigurationPcs.Add(config);
        await context.SaveChangesAsync();

         var commande = new Commande
            {
     UtilisateurId = 1,
            ConfigurationPcId = config.Id,
           PrixConfiguration = 1500.00m
         };

    // Act
            context.Commandes.Add(commande);
            await context.SaveChangesAsync();

            // Assert
         var saved = await context.Commandes
   .FirstOrDefaultAsync(c => c.UtilisateurId == 1 && c.PrixConfiguration == 1500.00m);
            
  Assert.NotNull(saved);
            Assert.Equal(1500.00m, saved.PrixConfiguration);
            Assert.Equal(config.Id, saved.ConfigurationPcId);
   }

        [Fact]
        public async Task GetCommandeWithRelations_ShouldIncludeNavigationProperties()
        {
     // Arrange
         using var context = _fixture.CreateContext();
          var config = new ConfigurationPc
            {
        NomConfiguration = "Gaming PC",
UtilisateurId = 1
            };
      context.ConfigurationPcs.Add(config);
   await context.SaveChangesAsync();

     var commande = new Commande
   {
         UtilisateurId = 1,
     ConfigurationPcId = config.Id,
          PrixConfiguration = 2000.00m
            };
         context.Commandes.Add(commande);
          await context.SaveChangesAsync();

        // Act
            var result = await context.Commandes
      .Include(c => c.Utilisateur)
       .Include(c => c.ConfigurationPc)
              .FirstOrDefaultAsync(c => c.Id == commande.Id);

  // Assert
    Assert.NotNull(result);
   Assert.NotNull(result.Utilisateur);
    Assert.NotNull(result.ConfigurationPc);
       Assert.Equal("Jean", result.Utilisateur.Prenom);
        Assert.Equal("Gaming PC", result.ConfigurationPc.NomConfiguration);
 }

        [Fact]
        public async Task UpdateCommande_ShouldModifyExistingCommande()
        {
       // Arrange
    using var context = _fixture.CreateContext();
  var config = new ConfigurationPc
     {
        NomConfiguration = "Update Test",
       UtilisateurId = 1
            };
            context.ConfigurationPcs.Add(config);
            await context.SaveChangesAsync();

    var commande = new Commande
            {
 UtilisateurId = 1,
        ConfigurationPcId = config.Id,
                PrixConfiguration = 1000.00m
    };
       context.Commandes.Add(commande);
       await context.SaveChangesAsync();

         // Act
      commande.PrixConfiguration = 1500.00m;
          context.Commandes.Update(commande);
   await context.SaveChangesAsync();

        // Assert
     var updated = await context.Commandes.FindAsync(commande.Id);
          Assert.NotNull(updated);
          Assert.Equal(1500.00m, updated.PrixConfiguration);
        }

    [Fact]
        public async Task DeleteCommande_ShouldRemoveFromDatabase()
     {
      // Arrange
            using var context = _fixture.CreateContext();
  var config = new ConfigurationPc
            {
     NomConfiguration = "Delete Test",
    UtilisateurId = 1
            };
    context.ConfigurationPcs.Add(config);
          await context.SaveChangesAsync();

   var commande = new Commande
            {
         UtilisateurId = 1,
         ConfigurationPcId = config.Id,
                PrixConfiguration = 800.00m
  };
       context.Commandes.Add(commande);
   await context.SaveChangesAsync();
      var commandeId = commande.Id;

        // Act
          context.Commandes.Remove(commande);
         await context.SaveChangesAsync();

   // Assert
        var deleted = await context.Commandes.FindAsync(commandeId);
            Assert.Null(deleted);
        }

        [Fact]
      public async Task GetCommandesByUtilisateur_ShouldReturnUserCommandes()
      {
         // Arrange
            using var context = _fixture.CreateContext();
            var config1 = new ConfigurationPc { NomConfiguration = "Config1", UtilisateurId = 1 };
    var config2 = new ConfigurationPc { NomConfiguration = "Config2", UtilisateurId = 1 };
      
          context.ConfigurationPcs.AddRange(config1, config2);
            await context.SaveChangesAsync();

      var commande1 = new Commande { UtilisateurId = 1, ConfigurationPcId = config1.Id, PrixConfiguration = 1000m };
     var commande2 = new Commande { UtilisateurId = 1, ConfigurationPcId = config2.Id, PrixConfiguration = 2000m };
  
       context.Commandes.AddRange(commande1, commande2);
            await context.SaveChangesAsync();

      // Act
            var commandes = await context.Commandes
            .Where(c => c.UtilisateurId == 1)
           .ToListAsync();

          // Assert
      Assert.NotEmpty(commandes);
  Assert.Contains(commandes, c => c.PrixConfiguration == 1000m);
            Assert.Contains(commandes, c => c.PrixConfiguration == 2000m);
      }
    }
}
