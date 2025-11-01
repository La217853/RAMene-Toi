using Xunit;
using Microsoft.AspNetCore.Mvc;
using RameneToi.Controllers;
using RameneToi.Models;
using RameneToi.Tests.Fixture;

namespace RameneToi.Tests.Integration.Controllers
{
    public class ComposantsControllerTests : IClassFixture<DatabaseFixture>
    {
   private readonly DatabaseFixture _fixture;

        public ComposantsControllerTests(DatabaseFixture fixture)
        {
      _fixture = fixture;
  }

   [Fact]
  public async Task GetComposants_ShouldReturnAllComposants()
     {
         // Arrange
using var context = _fixture.CreateContext();
            var controller = new ComposantsController(context);

     // Act
            var result = await controller.GetComposants();

     // Assert
      var actionResult = Assert.IsType<ActionResult<IEnumerable<Composant>>>(result);
   var composants = Assert.IsAssignableFrom<IEnumerable<Composant>>(actionResult.Value);
         Assert.NotNull(composants);
Assert.NotEmpty(composants);
        }

        [Fact]
    public async Task GetComposant_WithValidId_ShouldReturnComposant()
 {
 // Arrange
            using var context = _fixture.CreateContext();
        var controller = new ComposantsController(context);
       var composant = new Composant
   {
        Type = "GPU",
    Marque = "NVIDIA",
       Modele = "RTX 4070",
     Prix = 599.99f,
  Stock = 10,
      Score = 88
            };
 context.Composants.Add(composant);
await context.SaveChangesAsync();

       // Act
  var result = await controller.GetComposant(composant.Id);

 // Assert
 var actionResult = Assert.IsType<ActionResult<Composant>>(result);
  var returnedComposant = Assert.IsType<Composant>(actionResult.Value);
         Assert.Equal("RTX 4070", returnedComposant.Modele);
     Assert.Equal(599.99f, returnedComposant.Prix, precision: 2);
   }

  [Fact]
     public async Task GetComposant_WithInvalidId_ShouldReturnNotFound()
        {
     // Arrange
       using var context = _fixture.CreateContext();
   var controller = new ComposantsController(context);

// Act
      var result = await controller.GetComposant(99999);

 // Assert
       Assert.IsType<NotFoundResult>(result.Result);
        }

   [Fact]
   public async Task PostComposant_WithValidData_ShouldCreateComposant()
 {
       // Arrange
      using var context = _fixture.CreateContext();
         var controller = new ComposantsController(context);
 var composant = new Composant
     {
       Type = "SSD",
  Marque = "WD",
    Modele = "Black SN850X",
         Prix = 149.99f,
 Stock = 20,
Score = 90
};

            // Act
            var result = await controller.PostComposant(composant);

         // Assert
   var actionResult = Assert.IsType<ActionResult<Composant>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
 var createdComposant = Assert.IsType<Composant>(createdResult.Value);
       
        Assert.Equal("SSD", createdComposant.Type);
  Assert.Equal("WD", createdComposant.Marque);
   Assert.Equal("Black SN850X", createdComposant.Modele);
   Assert.True(createdComposant.Id > 0);
  }

  [Fact]
        public async Task PostComposant_WithNullData_ShouldReturnBadRequest()
  {
// Arrange
using var context = _fixture.CreateContext();
         var controller = new ComposantsController(context);

         // Act
var result = await controller.PostComposant(null);

// Assert
       Assert.IsType<BadRequestResult>(result.Result);
  }

      [Fact]
    public async Task PutComposant_WithValidData_ShouldUpdateComposant()
        {
  // Arrange
    using var context = _fixture.CreateContext();
       var controller = new ComposantsController(context);
      var composant = new Composant
       {
    Type = "RAM",
  Marque = "Corsair",
Modele = "Vengeance",
     Prix = 100f,
  Stock = 15,
     Score = 85
     };
context.Composants.Add(composant);
 await context.SaveChangesAsync();

   composant.Prix = 120f;
     composant.Stock = 10;

// Act
  var result = await controller.PutComposant(composant.Id, composant);

 // Assert
Assert.IsType<NoContentResult>(result);
       
       var updated = await context.Composants.FindAsync(composant.Id);
  Assert.Equal(120f, updated.Prix);
    Assert.Equal(10, updated.Stock);
        }

  [Fact]
  public async Task PutComposant_WithMismatchedId_ShouldReturnBadRequest()
  {
  // Arrange
    using var context = _fixture.CreateContext();
  var controller = new ComposantsController(context);
 var composant = new Composant
{
     Id = 1,
    Type = "Test",
      Marque = "Test",
      Modele = "Test",
     Prix = 100f,
        Stock = 5,
   Score = 80
 };

   // Act
 var result = await controller.PutComposant(999, composant);

  // Assert
Assert.IsType<BadRequestResult>(result);
     }

   [Fact]
      public async Task PutComposant_WithInvalidId_ShouldReturnNotFound()
{
  // Arrange
      using var context = _fixture.CreateContext();
var controller = new ComposantsController(context);
 var composant = new Composant
      {
    Id = 99999,
Type = "Test",
    Marque = "Test",
 Modele = "Test",
  Prix = 100f,
     Stock = 5,
  Score = 80
      };

  // Act
var result = await controller.PutComposant(99999, composant);

      // Assert
     Assert.IsType<NotFoundResult>(result);
        }

[Fact]
        public async Task DeleteComposant_WithValidId_ShouldRemoveComposant()
        {
    // Arrange
using var context = _fixture.CreateContext();
     var controller = new ComposantsController(context);
     var composant = new Composant
     {
  Type = "Case",
  Marque = "NZXT",
     Modele = "H510",
     Prix = 79.99f,
         Stock = 8,
       Score = 82
    };
context.Composants.Add(composant);
await context.SaveChangesAsync();
       var composantId = composant.Id;

         // Act
 var result = await controller.DeleteComposant(composantId);

        // Assert
     Assert.IsType<NoContentResult>(result);
       var deleted = await context.Composants.FindAsync(composantId);
 Assert.Null(deleted);
      }

 [Fact]
        public async Task DeleteComposant_WithInvalidId_ShouldReturnNotFound()
        {
        // Arrange
      using var context = _fixture.CreateContext();
   var controller = new ComposantsController(context);

  // Act
      var result = await controller.DeleteComposant(99999);

   // Assert
  Assert.IsType<NotFoundResult>(result);
    }

   [Fact]
        public async Task PostComposant_ShouldNotPersistId()
        {
 // Arrange
using var context = _fixture.CreateContext();
      var controller = new ComposantsController(context);
 var composant = new Composant
            {
  Id = 12345, // ID manuel ne devrait pas être persisté
        Type = "CPU",
    Marque = "AMD",
 Modele = "Ryzen 5 7600X",
    Prix = 299f,
          Stock = 12,
      Score = 86
    };

   // Act
     var result = await controller.PostComposant(composant);

 // Assert
            var actionResult = Assert.IsType<ActionResult<Composant>>(result);
var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
 var createdComposant = Assert.IsType<Composant>(createdResult.Value);
            
    // L'ID généré par la DB ne devrait pas être 12345
       Assert.NotEqual(12345, createdComposant.Id);
     }

  [Fact]
        public async Task PutComposant_ShouldUpdateAllEditableProperties()
        {
  // Arrange
      using var context = _fixture.CreateContext();
    var controller = new ComposantsController(context);
var composant = new Composant
     {
  Type = "Old Type",
   Marque = "Old Brand",
        Modele = "Old Model",
     Prix = 100f,
     Stock = 5,
    Score = 70
      };
  context.Composants.Add(composant);
await context.SaveChangesAsync();

      composant.Type = "New Type";
   composant.Marque = "New Brand";
       composant.Modele = "New Model";
          composant.Prix = 200f;
    composant.Stock = 15;
    composant.Score = 95;

// Act
      await controller.PutComposant(composant.Id, composant);

  // Assert
var updated = await context.Composants.FindAsync(composant.Id);
   Assert.Equal("New Type", updated.Type);
     Assert.Equal("New Brand", updated.Marque);
    Assert.Equal("New Model", updated.Modele);
       Assert.Equal(200f, updated.Prix);
    Assert.Equal(15, updated.Stock);
   Assert.Equal(95, updated.Score);
        }

   [Fact]
        public async Task PostComposant_WithZeroStock_ShouldBeAllowed()
        {
       // Arrange
     using var context = _fixture.CreateContext();
  var controller = new ComposantsController(context);
 var composant = new Composant
   {
 Type = "Monitor",
    Marque = "Samsung",
 Modele = "Odyssey G9",
 Prix = 1299f,
       Stock = 0, // En rupture
     Score = 94
       };

        // Act
      var result = await controller.PostComposant(composant);

  // Assert
       var actionResult = Assert.IsType<ActionResult<Composant>>(result);
 var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
       var createdComposant = Assert.IsType<Composant>(createdResult.Value);
       
 Assert.Equal(0, createdComposant.Stock);
        }
    }
}
