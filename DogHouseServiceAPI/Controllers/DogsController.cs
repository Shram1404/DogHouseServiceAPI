using DogHouseServiceAPI.Data;
using DogHouseServiceAPI.Dto;
using DogHouseServiceAPI.Models;
using DogHouseServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DogHouseServiceAPI.Controllers
{
    [ApiController]
    public class DogsController : Controller
    {
        private readonly IDogService _dogService;

        public DogsController(DogHouseServiceAPIContext context, IDogService dogService)
        {
            _dogService = dogService;
        }

        // GET: Dogs
        [HttpGet("Dogs")]
        public Task<IActionResult> GetDogs([FromQuery] DogsGetRequest request) // DogsGetRequest from Dto folder
        {
            IEnumerable<Dog>? dog;
            try
            {
                dog = _dogService.GetDogs(request);
            }
            catch (ArgumentException e)
            {
                return Task.FromResult<IActionResult>(BadRequest(e.Message));
            }
            if (dog == null)
                return Task.FromResult<IActionResult>(NotFound());

            return Task.FromResult<IActionResult>(Json(dog));
        }

        // POST: Dog (Max 1 entity per request)
        [HttpPost("Dog")]
        public async Task<IActionResult> PostDogAsync([FromBody] Dog dog)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _dogService.AddDogAsync(dog);
            return CreatedAtAction("GetDog", dog);
        }

    }
}
