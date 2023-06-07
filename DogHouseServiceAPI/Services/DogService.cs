using DogHouseServiceAPI.Data;
using DogHouseServiceAPI.Models;
using DogHouseServiceAPI.Dto;

namespace DogHouseServiceAPI.Services
{
    public class DogService : IDogService
    {
        private readonly DogHouseServiceAPIContext _context;

        public DogService(DogHouseServiceAPIContext context)
        {
            _context = context;
        }
        public IEnumerable<Dog> GetDogs(DogsGetRequest request)
        {
            var dogs = _context.Dog.AsQueryable();

            if (!string.IsNullOrEmpty(request.Attribute))
            {
                if (request.Order == "desc")
                {
                    dogs = request.Attribute switch
                    {
                        "name" => dogs.OrderByDescending(d => d.Name),
                        "color" => dogs.OrderByDescending(d => d.Color),
                        "tail_length" => dogs.OrderByDescending(d => d.TailLength),
                        "weight" => dogs.OrderByDescending(d => d.Weight),
                        _ => dogs
                    };
                }
                else
                {
                    dogs = request.Attribute switch
                    {
                        "name" => dogs.OrderBy(d => d.Name),
                        "color" => dogs.OrderBy(d => d.Color),
                        "tail_length" => dogs.OrderBy(d => d.TailLength),
                        "weight" => dogs.OrderBy(d => d.Weight),
                        _ => dogs
                    };
                }
            }

            var pagedDogs = dogs.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
            if(pagedDogs.Count() != 0)
                return pagedDogs;

            return null;
        }
        public async Task AddDogAsync(Dog dog)
        {
            _context.Dog.Add(dog);
            await _context.SaveChangesAsync();
        }
    }
}
