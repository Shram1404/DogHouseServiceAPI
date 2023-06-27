using DogHouseServiceAPI.Controllers;
using DogHouseServiceAPI.Data;
using DogHouseServiceAPI.Dto;
using DogHouseServiceAPI.Models;
using DogHouseServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DogHouseServiceAPI.Tests
{
    /// <summary>
    /// Test class for the DogController
    /// </summary>
    public class DogsControllerTests
    {
        private readonly DogsController _dogsController;
        private readonly Mock<IDogService> _dogServiceMock;

        /// <summary>
        /// Initializes instances and objects required to run unit tests on DogsController class.
        /// </summary>
        public DogsControllerTests()
        {
            _dogServiceMock = new Mock<IDogService>();
            _dogsController = new DogsController(new DogHouseServiceAPIContext(), _dogServiceMock.Object);
        }

        /// <summary>
        /// Ensures the GetDogs method returns a NotFound result when no dogs are found.
        /// </summary>
        [Fact]
        public async Task GetDogs_ReturnsNotFound_WhenDogIsNull()
        {
            // Arrange
            var request = new DogsGetRequest();

            _dogServiceMock.Setup(x => x.GetDogs(request)).Returns((IEnumerable<Dog>)null);

            // Act
            var result = await _dogsController.GetDogs(request);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Ensures the GetDogs method returns a JsonResult containing the dog information.
        /// </summary>
        [Fact]
        public async Task GetDogs_ReturnsJsonResult_WhenDogIsNotNull()
        {
            // Arrange
            var request = new DogsGetRequest();

            Dog dog = new()
            {
                Name = "Test DogTest Dog",
                Weight = 10,
                Color = "Brown",
                TailLength = 5
            };

            _dogServiceMock.Setup(x => x.GetDogs(request)).Returns(await Task.FromResult<IEnumerable<Dog>>(new List<Dog> { dog }));

            // Act
            var result = await _dogsController.GetDogs(request);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Single((IEnumerable<Dog>)jsonResult.Value);
            Assert.Equal(dog, ((IEnumerable<Dog>)jsonResult.Value).First());
        }

        /// <summary>
        /// Ensures the PostDogAsync method returns a BadRequestObjectResult when model state is invalid.
        /// </summary>
        [Fact]
        public async Task PostDogAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var invalidDog = new Dog();
            _dogsController.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await _dogsController.PostDogAsync(invalidDog);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
