
using Xunit;
using RameneToi.Models;

namespace RameneToi.Tests.Unit.Models
{
    public class CommandeTests
    {
        [Fact]
        public void Commande_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var commande = new Commande();

            // Assert
            Assert.Equal(0, commande.Id);
            Assert.Equal(0, commande.UtilisateurId);
            Assert.Equal(0, commande.ConfigurationPcId);
            Assert.Equal(0, commande.PrixConfiguration);
            Assert.Null(commande.ConfigurationPc);
            Assert.Null(commande.Utilisateur);
        }

        [Fact]
        public void Commande_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var commande = new Commande
            {
                Id = 1,
                UtilisateurId = 10,
                ConfigurationPcId = 5,
                PrixConfiguration = 1299.99m
            };

            // Assert
            Assert.Equal(1, commande.Id);
            Assert.Equal(10, commande.UtilisateurId);
            Assert.Equal(5, commande.ConfigurationPcId);
            Assert.Equal(1299.99m, commande.PrixConfiguration);
        }

        [Fact]
        public void Commande_ShouldHandleNavigationProperties()
        {
            // Arrange
            var utilisateur = new Utilisateurs { Id = 1, Nom = "Doe", Prenom = "John" };
            var config = new ConfigurationPc { Id = 1, NomConfiguration = "Gaming PC" };

            var commande = new Commande
            {
                UtilisateurId = 1,
                Utilisateur = utilisateur,
                ConfigurationPcId = 1,
                ConfigurationPc = config
            };

            // Assert
            Assert.NotNull(commande.Utilisateur);
            Assert.NotNull(commande.ConfigurationPc);
            Assert.Equal("Doe", commande.Utilisateur.Nom);
            Assert.Equal("Gaming PC", commande.ConfigurationPc.NomConfiguration);
        }
    }
}