using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace netcoreconf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        [HttpGet("name")]
        public IActionResult Name()
        {
            return Ok("NetCoreConf...");
        }

        [HttpGet("slowname")]
        public async Task<IActionResult> SlowName()
        {
            await Task.Delay(2000);
            return Ok("NetCoreConf...");
        }
    }
}
