using Microsoft.AspNetCore.Mvc;

namespace Apuestas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RandomController : ControllerBase
    {
        private static readonly Random _random = new Random();
        private static readonly Dictionary<int, string> NumberColorMapping = new Dictionary<int, string>
        {
            { 1, "rojo" }, { 2, "negro" }, { 3, "rojo" }, { 4, "negro" }, { 5, "rojo" },
            { 6, "negro" }, { 7, "rojo" }, { 8, "negro" }, { 9, "rojo" }, { 10, "negro" },
            { 11, "negro" }, { 12, "rojo" }, { 13, "negro" }, { 14, "rojo" }, { 15, "negro" },
            { 16, "rojo" }, { 17, "negro" }, { 18, "rojo" }, { 19, "rojo" }, { 20, "negro" },
            { 21, "rojo" }, { 22, "negro" }, { 23, "rojo" }, { 24, "negro" }, { 25, "rojo" },
            { 26, "negro" }, { 27, "rojo" }, { 28, "negro" }, { 29, "negro" }, { 30, "rojo" },
            { 31, "negro" }, { 32, "rojo" }, { 33, "negro" }, { 34, "rojo" }, { 35, "negro" },
            { 36, "rojo" }, { 0, "verde" }
        };

        [HttpGet]
        public IActionResult GetRandomNumberAndColor()
        {
            int number = _random.Next(0, 37);
            string color = NumberColorMapping[number];
            return Ok(new { number, color });
        }
    }
}
