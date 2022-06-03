using Blog.DataContext;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("posts")]
        public IActionResult Get([FromServices] BlogDataContext contex)
        {
            var post = Ok(contex.Posts.ToList());
            return Ok(post);
        }
    }
}
