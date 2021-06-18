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
    //[Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string param1, string param2, string param3)
        {
            //todo some logic
            return Ok("answer");
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            //todo some logic
            return Ok("answer");
        }

        /*
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
        */
    }
}
