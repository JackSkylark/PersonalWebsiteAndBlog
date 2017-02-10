using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Api.StaticContentApi.Controllers
{
    [Route("static-content")]
    public class StaticContentController : Controller
    {
        [Route("")]
        [HttpGet]
        [Produces(typeof(string))]
        public async Task<IActionResult> GetStaticContent()
        {
            return Ok("Success");
        }
    }
}
