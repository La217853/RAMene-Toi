using Xunit;
using RameneToi.Models;
using RameneToi.Tests.Fixture;
using Microsoft.EntityFrameworkCore;

namespace RameneToi.Tests.Integration.Repositories
{
    public class ComposantRepositoryTests : IClassFixture<DatabaseFixture>
    {
   private readonly DatabaseFixture _fixture;

 public ComposantRepositoryTests(DatabaseFixture fixture)
 {
       _fixture = fixture;
        }

   [Fact]
public async Task CreateComposant_ShouldSaveToDatabase()
  {
            // Arrange
    using var context = _fixture.CreateContext();
      var composant = new Composant
  {
     Type = "SSD",
     Marque = "Samsung",
       Modele = "980 Pro 2TB",
       Prix = 299.99f,
     Stock = 15,
         Score = 92
     };

   // Act
     context.Composants.Add(composant);
 await context.SaveChangesAsync();

      // Assert
   var saved = await context.Composants
     .FirstOrDefaultAsync(c => c.Modele == "980 Pro 2TB");

 Assert.NotNull(saved);
       Assert.Equal("SSD", saved.Type);
    Assert.Equal("Samsung", saved.Marque);
  Assert.Equal(299.99f, saved.Prix, precision: 2);
      }

        [Fact]
public async Task GetComposantsByType_ShouldFilterCorrectly()
    {
       // Arrange
   using var context = _fixture.CreateContext();
      var gpu1 = new Composant
   {
    Type = "GPU",
    Marque = "NVIDIA",
Modele = "RTX 4080",
   Prix = 1199.99f,
     Stock = 8,
         Score = 93
     };

 var gpu2 = new Composant
     {
   Type = "GPU",
  Marque = "AMD",
          Modele = "RX 7900 XT",
      Prix = 899.99f,
  Stock = 12,
        Score = 91
 };

       context.Composants.AddRange(gpu1, gpu2);
   await context.SaveChangesAsync();

// Act
var gpus = await context.Composants
        .Where(c => c.Type == "GPU")
      .ToListAsync();

       // Assert
     Assert.NotEmpty(gpus);
 Assert.All(gpus, gpu => Assert.Equal("GPU", gpu.Type));
   }

    [Fact]
public async Task UpdateComposant_ShouldModifyStock()
   {
     // Arrange
      using var context = _fixture.CreateContext();
 var composant = new Composant
        {
  Type = "Motherboard",
  Marque = "ASUS",
 Modele = "ROG Strix Z790",
     Prix = 499.99f,
Stock = 5,
    Score = 88
          };

   context.Composants.Add(composant);
    await context.SaveChangesAsync();

        // Act
       composant.Stock = 10;
       context.Composants.Update(composant);
      await context.SaveChangesAsync();

// Assert
var updated = await context.Composants.FindAsync(composant.Id);
Assert.NotNull(updated);
  Assert.Equal(10, updated.Stock);
     }

    [Fact]
   public async Task DeleteComposant_ShouldRemoveFromDatabase()
  {
    // Arrange
            using var context = _fixture.CreateContext();
 var composant = new Composant
       {
   Type = "PSU",
  Marque = "Corsair",
     Modele = "RM1000x",
       Prix = 199.99f,
    Stock = 7,
Score = 90
      };

  context.Composants.Add(composant);
   await context.SaveChangesAsync();
         var composantId = composant.Id;

     // Act
  context.Composants.Remove(composant);
 await context.SaveChangesAsync();

  // Assert
  var deleted = await context.Composants.FindAsync(composantId);
Assert.Null(deleted);
        }

  [Fact]
public async Task GetComposantWithConfigurations_ShouldIncludeManyToManyRelation()
   {
    // Arrange
      using var context = _fixture.CreateContext();
 var composant = new Composant
   {
 Type = "CPU",
 Marque = "Intel",
      Modele = "i7-13700K",
   Prix = 419.99f,
   Stock = 12,
  Score = 89
        };

    var config1 = new ConfigurationPc
 {
    NomConfiguration = "Intel Build 1",
  UtilisateurId = 1,
    Composants = new List<Composant> { composant }
    };

 var config2 = new ConfigurationPc
{
       NomConfiguration = "Intel Build 2",
  UtilisateurId = 1,
  Composants = new List<Composant> { composant }
            };

      context.Composants.Add(composant);
context.ConfigurationPcs.AddRange(config1, config2);
     await context.SaveChangesAsync();

// Act
     var result = await context.Composants
      .Include(c => c.Configurations)
     .FirstOrDefaultAsync(c => c.Id == composant.Id);

  // Assert
  Assert.NotNull(result);
Assert.NotNull(result.Configurations);
            Assert.Equal(2, result.Configurations.Count);
 }

  [Fact]
public async Task GetComposantsByPriceRange_ShouldFilterCorrectly()
  {
       // Arrange
      using var context = _fixture.CreateContext();
      var cheap = new Composant
      {
    Type = "Case",
     Marque = "Generic",
    Modele = "Basic Case",
Prix = 50f,
     Stock = 30,
Score = 70
  };

     var expensive = new Composant
{
    Type = "Monitor",
      Marque = "LG",
    Modele = "UltraGear 4K",
       Prix = 800f,
    Stock = 5,
      Score = 95
};

       context.Composants.AddRange(cheap, expensive);
            await context.SaveChangesAsync();

   // Act
 var affordableComponents = await context.Composants
.Where(c => c.Prix <= 100f)
      .ToListAsync();

       // Assert
Assert.Contains(affordableComponents, c => c.Modele == "Basic Case");
   Assert.DoesNotContain(affordableComponents, c => c.Modele == "UltraGear 4K");
        }

  [Fact]
public async Task GetComposantsByScore_ShouldOrderCorrectly()
  {
            // Arrange
using var context = _fixture.CreateContext();
 var comp1 = new Composant
   {
      Type = "GPU",
    Marque = "NVIDIA",
  Modele = "RTX 3060",
         Prix = 399f,
   Stock = 10,
Score = 80
      };

      var comp2 = new Composant
{
         Type = "GPU",
Marque = "NVIDIA",
      Modele = "RTX 4090",
  Prix = 1899f,
     Stock = 3,
       Score = 99
 };

      context.Composants.AddRange(comp1, comp2);
     await context.SaveChangesAsync();

// Act
var topScored = await context.Composants
     .Where(c => c.Type == "GPU")
            .OrderByDescending(c => c.Score)
      .FirstOrDefaultAsync();

     // Assert
Assert.NotNull(topScored);
    Assert.Equal("RTX 4090", topScored.Modele);
     Assert.Equal(99, topScored.Score);
      }

        [Fact]
public async Task CheckComposantStock_ShouldIdentifyOutOfStock()
        {
       // Arrange
      using var context = _fixture.CreateContext();
var inStock = new Composant
     {
    Type = "RAM",
       Marque = "G.Skill",
    Modele = "Trident Z5",
Prix = 189f,
      Stock = 15,
    Score = 88
   };

   var outOfStock = new Composant
{
        Type = "Cooler",
      Marque = "Noctua",
Modele = "NH-D15",
    Prix = 99f,
Stock = 0,
  Score = 92
 };

       context.Composants.AddRange(inStock, outOfStock);
await context.SaveChangesAsync();

// Act
 var unavailable = await context.Composants
    .Where(c => c.Stock == 0)
 .ToListAsync();

       // Assert
   Assert.Single(unavailable);
 Assert.Equal("NH-D15", unavailable.First().Modele);
}
    }
}
