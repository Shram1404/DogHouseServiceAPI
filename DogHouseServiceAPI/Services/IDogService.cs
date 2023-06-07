using DogHouseServiceAPI.Dto;
using DogHouseServiceAPI.Models;

namespace DogHouseServiceAPI.Services
{
    public interface IDogService
    {
        IEnumerable<Dog> GetDogs(DogsGetRequest request);
        Task AddDogAsync(Dog dog);
    }
}
