using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet("GetAdmin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdmin()
        {
            return Ok("Admin");
        }

        [HttpGet("GetUnauthorized")]
        [AllowAnonymous]
        public IActionResult GetUnauthorized()
        {
            return Ok("Unauthorized");
        }

        [HttpGet("GetAuthorized")]
        public IActionResult GetAuthorized()
        {
            return Ok("Authorized");
        }
    }
}
