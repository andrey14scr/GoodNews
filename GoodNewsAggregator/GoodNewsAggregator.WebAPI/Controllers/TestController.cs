using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet("GetDefault")]
        public IActionResult GetDefault()
        {
            return Ok("Authorized");
        }

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
    }
}
