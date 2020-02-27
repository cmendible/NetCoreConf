using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace netcoreconf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthzController : Controller
    {
        [HttpGet()]
        public IActionResult Get()
        {
            return Ok("I'm OK...");
        }
    }
}
