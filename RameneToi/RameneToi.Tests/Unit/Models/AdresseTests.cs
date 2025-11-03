using Xunit;
using RameneToi.Models;

namespace RameneToi.Tests.Unit.Models
{
    public class AdresseTests
    {
        [Fact]
        public void Adresse_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var adresse = new Adresse();

            // Assert
            Assert.Equal(0, adresse.Id);
            Assert.Equal(0, adresse.Code);
            Assert.Equal(0, adresse.Numero);
            Assert.Null(adresse.Rue);
            Assert.Null(adresse.utilisateur);
        }

        [Fact]
        public void Adresse_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var adresse = new Adresse
            {
                Id = 1,
                Code = 75001,
                Numero = 42,
                Rue = "Avenue des Champs-Élysées"
            };

            // Assert
            Assert.Equal(1, adresse.Id);
            Assert.Equal(75001, adresse.Code);
            Assert.Equal(42, adresse.Numero);
            Assert.Equal("Avenue des Champs-Élysées", adresse.Rue);
        }

        [Fact]
        public void Adresse_UtilisateurId_ShouldReturnEmptyListWhenUtilisateurIsNull()
        {
            // Arrange
            var adresse = new Adresse
            {
                Id = 1,
                utilisateur = null
            };

            // Act
            var utilisateurIds = adresse.UtilisateurId;

            // Assert
            Assert.NotNull(utilisateurIds);
            Assert.Empty(utilisateurIds);
        }

        [Fact]
        public void Adresse_UtilisateurId_ShouldReturnListOfUtilisateurIds()
        {
            // Arrange
            var adresse = new Adresse
            {
                Id = 1,
                Code = 75002,
                utilisateur = new List<Utilisateurs>
                {
                    new Utilisateurs { Id = 10, Nom = "Dupont" },
                    new Utilisateurs { Id = 20, Nom = "Martin" },
                    new Utilisateurs { Id = 30, Nom = "Bernard" }
                }
            };

            // Act
            var utilisateurIds = adresse.UtilisateurId;

            // Assert
            Assert.NotNull(utilisateurIds);
            Assert.Equal(3, utilisateurIds.Count);
            Assert.Contains(10, utilisateurIds);
            Assert.Contains(20, utilisateurIds);
            Assert.Contains(30, utilisateurIds);
        }

        [Fact]
        public void Adresse_ShouldHandleMultipleUtilisateurs()
        {
            // Arrange
            var utilisateur1 = new Utilisateurs { Id = 1, Prenom = "Jean", Nom = "Dupont" };
            var utilisateur2 = new Utilisateurs { Id = 2, Prenom = "Marie", Nom = "Curie" };

            var adresse = new Adresse
            {
                Id = 1,
                Code = 69001,
                Numero = 15,
                Rue = "Rue de la République",
                utilisateur = new List<Utilisateurs> { utilisateur1, utilisateur2 }
            };

            // Assert
            Assert.NotNull(adresse.utilisateur);
            Assert.Equal(2, adresse.utilisateur.Count);
            Assert.Contains(utilisateur1, adresse.utilisateur);
            Assert.Contains(utilisateur2, adresse.utilisateur);
        }
    }
}