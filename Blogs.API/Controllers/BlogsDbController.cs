using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Blogs.APP.Domain;

namespace Blogs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsDbController : ControllerBase
    {
        [HttpGet, Route("~/api/SeedDb")]
                                       
        public IActionResult Seed()
        {
            var dbFactory = new BlogsDbFactory();
            dbFactory.SeedDb();
            return Ok("Database seed successful.");
        }
    }
}
