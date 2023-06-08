using DogHouseServiceAPI.Data;
using DogHouseServiceAPI.Dto;
using DogHouseServiceAPI.Models;
using DogHouseServiceAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DogHouseServiceAPI.Tests
{
    /// <summary>
    /// Test class for the DogService class
    /// </summary>
    public class DogServiceTests
    {
        private List<Dog> _dogs;
        private Mock<DbSet<Dog>> _mockSet;
        private Mock<DogHouseServiceAPIContext> _mockContext;

        /// <summary>
        /// Initializes the test class
        /// </summary>
        public DogServiceTests()
        {
            _dogs = new List<Dog>
            {
                new Dog { Name = "Dog 1", Color = "Black", TailLength = 10, Weight = 20 },
                new Dog { Name = "Dog 2", Color = "White", TailLength = 5},
                new Dog { Name = "Dog 3", Color = "Gray", TailLength = 5},
            };
            // Setting up the mock DbSet
            _mockSet = new Mock<DbSet<Dog>>();
            _mockSet.As<IQueryable<Dog>>().Setup(m => m.Provider).Returns(_dogs.AsQueryable().Provider);
            _mockSet.As<IQueryable<Dog>>().Setup(m => m.Expression).Returns(_dogs.AsQueryable().Expression);
            _mockSet.As<IQueryable<Dog>>().Setup(m => m.ElementType).Returns(_dogs.AsQueryable().ElementType);
            _mockSet.As<IQueryable<Dog>>().Setup(m => m.GetEnumerator()).Returns(_dogs.GetEnumerator());
            _mockSet.Setup(m => m.Add(It.IsAny<Dog>())).Callback<Dog>(d => _dogs.Add(d));
            _mockSet.Setup(m => m.AsQueryable()).Returns(_dogs.AsQueryable());

            _mockContext = new Mock<DogHouseServiceAPIContext>();
            _mockContext.Setup(c => c.Dog).Returns(_mockSet.Object);
        }

        /// <summary>
        /// Tests that the GetDogs method returns null when no dogs are found
        /// </summary>
        [Fact]
        public void GetDogs_ReturnsNull_WhenNoDogsFound()
        {
            // Arrange
            _dogs.Clear();
            var service = new DogService(_mockContext.Object);

            // Act
            var result = service.GetDogs(new DogsGetRequest());

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests that the GetDogs method returns filtered dogs when dogs are found
        /// </summary>
        [Fact]
        public void GetDogs_ReturnsFilteredDogs_WhenDogsFound()
        {
            // Arrange
            var service = new DogService(_mockContext.Object);
            var request = new DogsGetRequest { Attribute = "color", Order = "desc", PageNumber = 1, PageSize = 2 };

            // Act
            var result = service.GetDogs(request);

            // Assert
            Assert.NotNull(result);
            var resultList = Assert.IsAssignableFrom<IEnumerable<Dog>>(result);
            Assert.Equal(2, resultList.Count());
            Assert.Equal("White", resultList.First().Color);
            Assert.Equal("Gray", resultList.Last().Color);
        }

        /// <summary>
        /// Tests that the AddDogAsync method adds a new dog to the Dog list
        /// </summary>
        [Fact]
        public async Task AddDogAsync_AddsNewDog()
        {
            // Arrange
            var service = new DogService(_mockContext.Object);

            var newDog = new Dog { Name = "New Dog" };

            // Act
            await service.AddDogAsync(newDog);

            // Assert
            Assert.Contains(_dogs, d => d.Name == "New Dog");
            Assert.Contains(_dogs, d => d.Color == null);
            Assert.Contains(_dogs, d => d.TailLength == 0);
        }
    }
}
