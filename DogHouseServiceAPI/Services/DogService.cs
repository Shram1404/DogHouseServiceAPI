using DogHouseServiceAPI.Data;
using DogHouseServiceAPI.Dto;
using DogHouseServiceAPI.Models;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace DogHouseServiceAPI.Services
{
    // Service for organizing a list of dogs
    public class DogService : IDogService
    {
        private readonly DogHouseServiceAPIContext _context;

        public DogService(DogHouseServiceAPIContext context)
        {
            _context = context;
        }

        // Retrieve a list of dogs for the given request
        public IEnumerable<Dog>? GetDogs(DogsGetRequest request)
        {
            IQueryable<Dog> dogs = _context.Dog.AsQueryable();

            // If request contains an attribute, sort accordingly
            if (!string.IsNullOrEmpty(request.Attribute))
            {
                bool descending = request.Order == "desc";

                // Check if the requested attribute exists in the Dog class
                PropertyInfo? attributeProperty = typeof(Dog).GetProperty(request.Attribute, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (attributeProperty != null)
                {
                    string orderByExpression = $"{request.Attribute} {(descending ? "descending" : "ascending")}";
                    dogs = dogs.OrderBy(orderByExpression);
                }
                else throw new ArgumentException("Invalid attribute specified for dog sorting");
            }
            // Paginate and return dogs
            IQueryable<Dog> pagedDogs = dogs.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
            if (pagedDogs.Any())
                return pagedDogs;

            return null;
        }

        // Add a dog to the database asynchronously
        public async Task AddDogAsync(Dog dog)
        {
            _context.Dog.Add(dog);
            await _context.SaveChangesAsync();
        }
    }
}
