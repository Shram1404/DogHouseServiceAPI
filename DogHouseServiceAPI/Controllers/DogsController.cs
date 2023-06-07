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

        [HttpGet]
        public IActionResult Index() =>
            Ok("Lol");

        // GET: Dogs
        [HttpGet("Dogs")]
        public Task<IActionResult> GetDogs([FromQuery] DogsGetRequest request)
        {
            var dog = _dogService.GetDogs(request);

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