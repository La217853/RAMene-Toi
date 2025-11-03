using Xunit;
using RameneToi.Models;

namespace RameneToi.Tests.Unit.Models
{
    public class ComposantTests
    {
        [Fact]
        public void Composant_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var composant = new Composant();

            // Assert
            Assert.Equal(0, composant.Id);
            Assert.Null(composant.Type);
            Assert.Null(composant.Marque);
            Assert.Null(composant.Modele);
            Assert.Equal(0f, composant.Prix);
            Assert.Equal(0, composant.Stock);
            Assert.Equal(0, composant.Score);
            Assert.Null(composant.Configurations);
        }

        [Fact]
        public void Composant_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var composant = new Composant
            {
                Id = 1,
                Type = "GPU",
                Marque = "NVIDIA",
                Modele = "RTX 4090",
                Prix = 1899.99f,
                Stock = 15,
                Score = 95
            };

            // Assert
            Assert.Equal(1, composant.Id);
            Assert.Equal("GPU", composant.Type);
            Assert.Equal("NVIDIA", composant.Marque);
            Assert.Equal("RTX 4090", composant.Modele);
            Assert.Equal(1899.99f, composant.Prix);
            Assert.Equal(15, composant.Stock);
            Assert.Equal(95, composant.Score);
        }

        [Fact]
        public void Composant_ShouldHandleDifferentComponentTypes()
        {
            // Arrange
            var cpu = new Composant
            {
                Type = "CPU",
                Marque = "AMD",
                Modele = "Ryzen 9 7950X",
                Prix = 699.99f,
                Stock = 20,
                Score = 92
            };

            var ram = new Composant
            {
                Type = "RAM",
                Marque = "Corsair",
                Modele = "Vengeance DDR5 32GB",
                Prix = 149.99f,
                Stock = 50,
                Score = 88
            };

            // Assert
            Assert.Equal("CPU", cpu.Type);
            Assert.Equal("AMD", cpu.Marque);
            Assert.Equal("RAM", ram.Type);
            Assert.Equal("Corsair", ram.Marque);
        }

        [Fact]
        public void Composant_ShouldHandleConfigurationsNavigation()
        {
            // Arrange
            var config1 = new ConfigurationPc { Id = 1, NomConfiguration = "Gaming PC" };
            var config2 = new ConfigurationPc { Id = 2, NomConfiguration = "Workstation" };

            var composant = new Composant
            {
                Id = 1,
                Type = "SSD",
                Marque = "Samsung",
                Modele = "980 Pro 1TB",
                Configurations = new List<ConfigurationPc> { config1, config2 }
            };

            // Assert
            Assert.NotNull(composant.Configurations);
            Assert.Equal(2, composant.Configurations.Count);
            Assert.Contains(config1, composant.Configurations);
            Assert.Contains(config2, composant.Configurations);
        }

        [Fact]
        public void Composant_ShouldHandleNullConfigurationsList()
        {
            // Arrange
            var composant = new Composant
            {
                Id = 1,
                Type = "Motherboard",
                Marque = "ASUS",
                Modele = "ROG Maximus Z790",
                Configurations = null
            };

            // Assert
            Assert.Null(composant.Configurations);
        }

        [Fact]
        public void Composant_ShouldHandleZeroStock()
        {
            // Arrange
            var composant = new Composant
            {
                Id = 1,
                Type = "PSU",
                Marque = "Corsair",
                Modele = "RM850x",
                Prix = 149.99f,
                Stock = 0,
                Score = 85
            };

            // Assert
            Assert.Equal(0, composant.Stock);
        }

        [Fact]
        public void Composant_ShouldHandlePriceAsFloat()
        {
            // Arrange
            var composant = new Composant
            {
                Prix = 999.99f
            };

            // Assert
            Assert.IsType<float>(composant.Prix);
            Assert.Equal(999.99f, composant.Prix, precision: 2);
        }
    }
}