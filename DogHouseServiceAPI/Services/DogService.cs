using DogHouseServiceAPI.Data;
using DogHouseServiceAPI.Models;
using DogHouseServiceAPI.Dto;

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
            var dogs = _context.Dog.AsQueryable();

            // If request contains an attribute, sort accordingly
            if (!string.IsNullOrEmpty(request.Attribute))
            {
                var descending = request.Order == "desc";
                dogs = request.Attribute switch
                {
                    "name" => descending ? dogs.OrderByDescending(d => d.Name) : dogs.OrderBy(d => d.Name),
                    "color" => descending ? dogs.OrderByDescending(d => d.Color) : dogs.OrderBy(d => d.Color),
                    "tail_length" => descending ? dogs.OrderByDescending(d => d.TailLength) : dogs.OrderBy(d => d.TailLength),
                    "weight" => descending ? dogs.OrderByDescending(d => d.Weight) : dogs.OrderBy(d => d.Weight),
                    _ => dogs
                };
            }

            // Paginate and return dogs
            var pagedDogs = dogs.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
            if (pagedDogs.Count() > 0)
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
