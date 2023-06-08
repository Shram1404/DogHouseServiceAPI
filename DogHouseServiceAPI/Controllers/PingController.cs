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

        // GET: Ping
        [HttpGet("Ping")]
        public IActionResult Ping() => // Version contains in appsettings.json
            Ok("Dogs house service. Version " + _configuration.GetValue<string>("ApiVersion"));
    }
}
