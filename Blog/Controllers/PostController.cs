using Blog.DataContext;
using Blog.Models;
using Blog.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetPost([FromServices] BlogDataContext contex, [FromQuery] int page = 0, [FromQuery] int pageSize = 25)
        {
            try
            {
                var post =
               await contex
               .Posts
               .AsNoTracking()
               .Include(x => x.Category)
               .Include(x => x.Author)
               .Select(x => new ViewPostsModels
               {
                   Id = x.Id,
                   Title = x.Title,
                   Category = x.Category.Name,
                   Author = $"Email do author : {x.Author.Email}"

               })
               .Skip(page * pageSize)
               .Take(pageSize)
               .ToListAsync();

                if (post == null)
                    return NotFound(new ResultViewModel<string>("FG98-T Usuario Nulo"));

                return Ok(post);
            }
            catch (Exception)
            {

                return StatusCode(500, new ResultViewModel<string>("45DF-G Falha na busca dos dados"));
            }
        }
        [HttpGet("v1/posts/{id:int}")]
        public async Task<IActionResult> DetailsAsync([FromServices] BlogDataContext contex, [FromRoute] int id)
        {
            try
            {
                var post =
               await contex
               .Posts
               .AsNoTracking()
               .Include(x => x.Author)
               .ThenInclude(x => x.Roles)
               .Include(x => x.Category)
               .FirstOrDefaultAsync(x => x.Id == id);
               
                if (post == null)
                    return NotFound(new ResultViewModel<Post>("FG98-T Conteudo Nulo"));

                return Ok(post);
            }
            catch (Exception)
            {

                return StatusCode(500, new ResultViewModel<Post>("45DF-G Falha na busca dos dados"));
            }

        }
    }
}
