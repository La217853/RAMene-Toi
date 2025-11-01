using Xunit;
using RameneToi.Models;

namespace RameneToi.Tests.Unit.Models
{
    public class UtilisateursTests
    {
        [Fact]
        public void Utilisateurs_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var utilisateur = new Utilisateurs();

            // Assert
            Assert.Equal(0, utilisateur.Id);
            Assert.Null(utilisateur.Prenom);
            Assert.Null(utilisateur.Nom);
            Assert.Null(utilisateur.Email);
            Assert.Null(utilisateur.MotDePasse);
            Assert.Equal(0, utilisateur.AdresseId);
            Assert.Null(utilisateur.Adresse);
            Assert.Null(utilisateur.ConfigurationsPc);
            Assert.Null(utilisateur.Commandes);
        }

        [Fact]
        public void Utilisateurs_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var utilisateur = new Utilisateurs
            {
                Id = 1,
                Prenom = "Jean",
                Nom = "Dupont",
                Email = "jean.dupont@example.com",
                MotDePasse = "Password123!",
                AdresseId = 5
            };

            // Assert
            Assert.Equal(1, utilisateur.Id);
            Assert.Equal("Jean", utilisateur.Prenom);
            Assert.Equal("Dupont", utilisateur.Nom);
            Assert.Equal("jean.dupont@example.com", utilisateur.Email);
            Assert.Equal("Password123!", utilisateur.MotDePasse);
            Assert.Equal(5, utilisateur.AdresseId);
        }

        [Fact]
        public void Utilisateurs_ShouldHandleAdresseNavigationProperty()
        {
            // Arrange
            var adresse = new Adresse
            {
                Id = 1,
                Code = 75001,
                Numero = 10,
                Rue = "Rue de la Paix"
            };

            var utilisateur = new Utilisateurs
            {
                Id = 1,
                Prenom = "Marie",
                Nom = "Martin",
                AdresseId = 1,
                Adresse = adresse
            };

            // Assert
            Assert.NotNull(utilisateur.Adresse);
            Assert.Equal(75001, utilisateur.Adresse.Code);
            Assert.Equal("Rue de la Paix", utilisateur.Adresse.Rue);
        }

        [Fact]
        public void Utilisateurs_CommandeId_ShouldReturnEmptyListWhenCommandesIsNull()
        {
            // Arrange
            var utilisateur = new Utilisateurs
            {
                Id = 1,
                Commandes = null
            };

            // Act
            var commandeIds = utilisateur.CommandeId;

            // Assert
            Assert.NotNull(commandeIds);
            Assert.Empty(commandeIds);
        }

        [Fact]
        public void Utilisateurs_CommandeId_ShouldReturnListOfCommandeIds()
        {
            // Arrange
            var utilisateur = new Utilisateurs
            {
                Id = 1,
                Commandes = new List<Commande>
                {
                    new Commande { Id = 10 },
                    new Commande { Id = 20 },
                    new Commande { Id = 30 }
                }
            };

            // Act
            var commandeIds = utilisateur.CommandeId;

            // Assert
            Assert.NotNull(commandeIds);
            Assert.Equal(3, commandeIds.Count);
            Assert.Contains(10, commandeIds);
            Assert.Contains(20, commandeIds);
            Assert.Contains(30, commandeIds);
        }

        [Fact]
        public void Utilisateurs_ShouldHandleMultipleConfigurationsPc()
        {
            // Arrange
            var config1 = new ConfigurationPc { Id = 1, NomConfiguration = "PC Gaming" };
            var config2 = new ConfigurationPc { Id = 2, NomConfiguration = "PC Bureau" };

            var utilisateur = new Utilisateurs
            {
                Id = 1,
                ConfigurationsPc = new List<ConfigurationPc> { config1, config2 }
            };

            // Assert
            Assert.NotNull(utilisateur.ConfigurationsPc);
            Assert.Equal(2, utilisateur.ConfigurationsPc.Count);
            Assert.Contains(config1, utilisateur.ConfigurationsPc);
            Assert.Contains(config2, utilisateur.ConfigurationsPc);
        }
    }
}