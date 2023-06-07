using DogHouseServiceAPI.Data;
using DogHouseServiceAPI.Dto;
using DogHouseServiceAPI.Models;
using DogHouseServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DogHouseServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DogsController : Controller
    {
        private readonly DogHouseServiceAPIContext _context;
        private readonly IDogService _dogService;

        public DogsController(DogHouseServiceAPIContext context, IDogService dogService)
        {
            _context = context;
            _dogService = dogService;
        }

        // GET: Dogs
        [HttpGet("Dogs")]
        public Task<IActionResult> GetDogs([FromQuery] DogsGetRequest request)
        {
            var dog = _dogService.GetDogs(request);
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