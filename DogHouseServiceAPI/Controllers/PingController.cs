using Microsoft.AspNetCore.Mvc;

namespace DogHouseServiceAPI.Controllers
{
    public class PingController : Controller
    {
        [HttpGet("Ping")]
        public IActionResult Index() => Ok("Dogs house service. Version 1.0.1");//додати глобально в проект
    }
}
