using Microsoft.AspNetCore.Mvc;
using System.Configuration;

namespace DogHouseServiceAPI.Controllers
{
    public class PingController : Controller
    {
        private readonly IConfiguration _configuration;
        public PingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("Ping")]
        public IActionResult Ping() =>
            Ok("Dogs house service. Version " + _configuration.GetValue<string>("ApiVersion"));
    }
}
