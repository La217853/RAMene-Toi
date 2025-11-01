using Xunit;
using RameneToi.Models;

namespace RameneToi.Tests.Unit.Models
{
    public class ConfigurationPcTests
    {
        [Fact]
        public void ConfigurationPc_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var config = new ConfigurationPc();

            // Assert
            Assert.Equal(0, config.Id);
            Assert.Null(config.NomConfiguration);
            Assert.Equal(0, config.UtilisateurId);
            Assert.Null(config.Utilisateur);
            Assert.NotNull(config.Composants);
            Assert.Empty(config.Composants);
            Assert.Null(config.Commande);
        }

        [Fact]
        public void ConfigurationPc_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var config = new ConfigurationPc
            {
                Id = 1,
                NomConfiguration = "PC Gaming Ultra",
                UtilisateurId = 10
            };

            // Assert
            Assert.Equal(1, config.Id);
            Assert.Equal("PC Gaming Ultra", config.NomConfiguration);
            Assert.Equal(10, config.UtilisateurId);
        }

        [Fact]
        public void ConfigurationPc_ShouldHandleUtilisateurNavigationProperty()
        {
            // Arrange
            var utilisateur = new Utilisateurs
            {
                Id = 5,
                Prenom = "Alice",
                Nom = "Durand"
            };

            var config = new ConfigurationPc
            {
                Id = 1,
                NomConfiguration = "PC Professionnel",
                UtilisateurId = 5,
                Utilisateur = utilisateur
            };

            // Assert
            Assert.NotNull(config.Utilisateur);
            Assert.Equal(5, config.Utilisateur.Id);
            Assert.Equal("Alice", config.Utilisateur.Prenom);
            Assert.Equal("Durand", config.Utilisateur.Nom);
        }

        [Fact]
        public void ConfigurationPc_ShouldHandleComposantsList()
        {
            // Arrange
            var composant1 = new Composant
            {
                Id = 1,
                Type = "CPU",
                Marque = "Intel",
                Modele = "i9-13900K"
            };

            var composant2 = new Composant
            {
                Id = 2,
                Type = "GPU",
                Marque = "NVIDIA",
                Modele = "RTX 4090"
            };

            var config = new ConfigurationPc
            {
                Id = 1,
                NomConfiguration = "Gaming Setup",
                Composants = new List<Composant> { composant1, composant2 }
            };

            // Assert
            Assert.NotNull(config.Composants);
            Assert.Equal(2, config.Composants.Count);
            Assert.Contains(composant1, config.Composants);
            Assert.Contains(composant2, config.Composants);
        }

        [Fact]
        public void ConfigurationPc_ShouldHandleCommandeRelation()
        {
            // Arrange
            var commande = new Commande
            {
                Id = 1,
                ConfigurationPcId = 1,
                PrixConfiguration = 2500.00m
            };

            var config = new ConfigurationPc
            {
                Id = 1,
                NomConfiguration = "PC Custom",
                Commande = commande
            };

            // Assert
            Assert.NotNull(config.Commande);
            Assert.Equal(1, config.Commande.Id);
            Assert.Equal(2500.00m, config.Commande.PrixConfiguration);
        }

        [Fact]
        public void ConfigurationPc_Commande_ShouldBeNullableForOneToZeroOrOneRelation()
        {
            // Arrange
            var config = new ConfigurationPc
            {
                Id = 1,
                NomConfiguration = "PC Sans Commande",
                Commande = null
            };

            // Assert
            Assert.Null(config.Commande);
        }
    }
}