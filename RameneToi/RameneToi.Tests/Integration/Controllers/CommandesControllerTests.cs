using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RameneToi.Controllers;
using RameneToi.Data;
using RameneToi.Models;
using RameneToi.Tests.Fixture;
using Xunit;

namespace RameneToi.Tests.Integration.Controllers
{
    public class CommandesControllerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

 public CommandesControllerTests(DatabaseFixture fixture)
      {
     _fixture = fixture;
        }

        private CommandesController CreateControllerWithAdminUser(RameneToiWebAPIContext context)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.Name, "admin"),
        new Claim(ClaimTypes.Role, "Admin")
    }, "TestAuthentication"));

            // Simule un token que ton AuthorizationService reconnaît comme valide
            httpContext.Request.Headers["Authorization"] = "Bearer test-admin-token";

            var controller = new CommandesController(context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };

            return controller;
        }


        [Fact]
        public async Task GetCommande_ShouldReturnAllCommandes()
        {

      // Arrange
     using var context = _fixture.CreateContext();
            var controller = CreateControllerWithAdminUser(context);
            

     // Act
            var result = await controller.GetCommande();

            // Assert
       var actionResult = Assert.IsType<ActionResult<IEnumerable<Commande>>>(result);
   var commandes = Assert.IsAssignableFrom<IEnumerable<Commande>>(actionResult.Value);
            Assert.NotNull(commandes);
        }

        [Fact]
        public async Task GetCommande_WithValidId_ShouldReturnCommande()
      {
            // Arrange
using var context = _fixture.CreateContext();
  var controller = new CommandesController(context);
        var config = new ConfigurationPc
 {
 NomConfiguration = "Test Config",
  UtilisateurId = 1
      };
         context.ConfigurationPcs.Add(config);
       await context.SaveChangesAsync();

       var commande = new Commande
      {
  UtilisateurId = 1,
    ConfigurationPcId = config.Id,
         PrixConfiguration = 1000m
            };
          context.Commandes.Add(commande);
       await context.SaveChangesAsync();

     // Act
            var result = await controller.GetCommande(commande.Id);

            // Assert
      var actionResult = Assert.IsType<ActionResult<Commande>>(result);
            var returnedCommande = Assert.IsType<Commande>(actionResult.Value);
     Assert.Equal(commande.Id, returnedCommande.Id);
 Assert.Equal(1000m, returnedCommande.PrixConfiguration);
        }

        [Fact]
        public async Task GetCommande_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
 using var context = _fixture.CreateContext();
      var controller = new CommandesController(context);

     // Act
        var result = await controller.GetCommande(99999);

         // Assert
 Assert.IsType<NotFoundResult>(result.Result);
   }

        [Fact]
        public async Task PostCommande_ShouldCalculatePriceWithTVA()
        {
         // Arrange
            using var context = _fixture.CreateContext();
            var controller = new CommandesController(context);
     var composant1 = new Composant
     {
   Type = "GPU",
       Marque = "NVIDIA",
    Modele = "RTX 4090",
             Prix = 1000f,
Stock = 5,
     Score = 95
   };

      var composant2 = new Composant
            {
     Type = "CPU",
                Marque = "Intel",
   Modele = "i9-13900K",
   Prix = 500f,
     Stock = 10,
     Score = 93
            };

  var config = new ConfigurationPc
     {
       NomConfiguration = "Gaming Setup",
     UtilisateurId = 1,
       Composants = new List<Composant> { composant1, composant2 }
      };
            
     context.Composants.AddRange(composant1, composant2);
            context.ConfigurationPcs.Add(config);
            await context.SaveChangesAsync();

        var commande = new Commande
 {
     UtilisateurId = 1,
    ConfigurationPcId = config.Id
   };

 // Act
            var result = await controller.PostCommande(commande);

     // Assert
            var actionResult = Assert.IsType<ActionResult<Commande>>(result);
          var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
  var createdCommande = Assert.IsType<Commande>(createdResult.Value);
        
            // 1500 HT + 21% TVA = 1815
            Assert.Equal(1815m, createdCommande.PrixConfiguration);
    }

   [Fact]
        public async Task PostCommande_WithInvalidConfigId_ShouldReturnBadRequest()
  {
            // Arrange
       using var context = _fixture.CreateContext();
       var controller = new CommandesController(context);
            var commande = new Commande
    {
        UtilisateurId = 1,
                ConfigurationPcId = 99999
     };

            // Act
         var result = await controller.PostCommande(commande);

       // Assert
       var actionResult = Assert.IsType<ActionResult<Commande>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
       Assert.Contains("introuvable", badRequestResult.Value.ToString());
        }

        [Fact]
    public async Task PostCommande_WithEmptyConfiguration_ShouldCalculateZeroPrice()
   {
            // Arrange
            using var context = _fixture.CreateContext();
            var controller = new CommandesController(context);
          var config = new ConfigurationPc
        {
NomConfiguration = "Empty Config",
         UtilisateurId = 1,
     Composants = new List<Composant>()
            };
    context.ConfigurationPcs.Add(config);
            await context.SaveChangesAsync();

            var commande = new Commande
      {
  UtilisateurId = 1,
                ConfigurationPcId = config.Id
 };

            // Act
         var result = await controller.PostCommande(commande);

    // Assert
    var actionResult = Assert.IsType<ActionResult<Commande>>(result);
var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
          var createdCommande = Assert.IsType<Commande>(createdResult.Value);
    Assert.Equal(0m, createdCommande.PrixConfiguration);
  }

        [Fact]
        public async Task PutCommande_WithValidId_ShouldUpdateCommande()
        {
            // Arrange
       using var context = _fixture.CreateContext();
            var controller = new CommandesController(context);
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
  PrixConfiguration = 1000m
       };
context.Commandes.Add(commande);
            await context.SaveChangesAsync();

   commande.PrixConfiguration = 2000m;

            // Act
      var result = await controller.PutCommande(commande.Id, commande);

// Assert
       Assert.IsType<NoContentResult>(result);
            var updated = await context.Commandes.FindAsync(commande.Id);
          Assert.Equal(2000m, updated.PrixConfiguration);
  }

        [Fact]
      public async Task PutCommande_WithMismatchedId_ShouldReturnBadRequest()
        {
     // Arrange
            using var context = _fixture.CreateContext();
            var controller = new CommandesController(context);
        var commande = new Commande
            {
       Id = 1,
      UtilisateurId = 1,
  ConfigurationPcId = 1,
     PrixConfiguration = 1000m
            };

        // Act
            var result = await controller.PutCommande(999, commande);

            // Assert
      Assert.IsType<BadRequestResult>(result);
        }

 [Fact]
        public async Task DeleteCommande_WithValidId_ShouldRemoveCommande()
        {
         // Arrange
     using var context = _fixture.CreateContext();
            var controller = new CommandesController(context);
            var config = new ConfigurationPc
          {
       NomConfiguration = "To Delete",
           UtilisateurId = 1
         };
            context.ConfigurationPcs.Add(config);
            await context.SaveChangesAsync();

            var commande = new Commande
         {
        UtilisateurId = 1,
        ConfigurationPcId = config.Id,
        PrixConfiguration = 500m
       };
    context.Commandes.Add(commande);
    await context.SaveChangesAsync();
         var commandeId = commande.Id;

   // Act
    var result = await controller.DeleteCommande(commandeId);

   // Assert
      Assert.IsType<NoContentResult>(result);
            var deletedCommande = await context.Commandes.FindAsync(commandeId);
            Assert.Null(deletedCommande);
        }

        [Fact]
   public async Task DeleteCommande_WithInvalidId_ShouldReturnNotFound()
      {
      // Arrange
            using var context = _fixture.CreateContext();
            var controller = new CommandesController(context);

       // Act
   var result = await controller.DeleteCommande(99999);

    // Assert
     Assert.IsType<NotFoundResult>(result);
 }

        [Fact]
        public async Task PostCommande_ShouldVerifyTVACalculation_21Percent()
        {
      // Arrange
            using var context = _fixture.CreateContext();
   var controller = new CommandesController(context);
     var composant = new Composant
            {
     Type = "Test",
           Marque = "Test",
            Modele = "Test Model",
                Prix = 100f,
    Stock = 1,
Score = 80
         };

      var config = new ConfigurationPc
          {
  NomConfiguration = "TVA Test",
          UtilisateurId = 1,
     Composants = new List<Composant> { composant }
    };

            context.Composants.Add(composant);
          context.ConfigurationPcs.Add(config);
  await context.SaveChangesAsync();

            var commande = new Commande
            {
    UtilisateurId = 1,
         ConfigurationPcId = config.Id
        };

            // Act
  var result = await controller.PostCommande(commande);

          // Assert
            var actionResult = Assert.IsType<ActionResult<Commande>>(result);
    var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
         var createdCommande = Assert.IsType<Commande>(createdResult.Value);
        
  // 100 HT * 1.21 = 121
       Assert.Equal(121m, createdCommande.PrixConfiguration);
        }
    }
}
