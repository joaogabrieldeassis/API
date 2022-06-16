using Blog.DataContext;
using Blog.Models;
using Blog.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class UserControler : ControllerBase
    {
        [HttpGet("v1/listuser")]
        public async Task<IActionResult> GetUser([FromServices] BlogDataContext context)
        {
            var receiveUser = await context.Users.ToListAsync();
            if (receiveUser == null)
                return StatusCode(500, new ResultViewModel<User>("Falha ao encontrar os usuarios"));
            return Ok(new ResultViewModel<List<User>>(receiveUser));
        }

        [HttpGet("v1/listuser/{id:int}")]
        public async Task<IActionResult> GetIdAsync([FromRoute] int id, [FromServices] BlogDataContext context)
        {
            var userId = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userId == null)
                return StatusCode(500, new ResultViewModel<User>("Ur4 - X Falha ao encontrar o usuario"));


            return Ok(new ResultViewModel<User>(userId));
        }
        [HttpDelete("v1/deletUser/id:int")]
        public async Task<IActionResult> DeletUser([FromBody] int id, [FromServices] BlogDataContext context)
        {
            try
            {
                var deleteUser = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (deleteUser == null)
                    return NotFound(new ResultViewModel<User>("FGT4 - G Usuario não encontrado"));

                context.Users.Remove(deleteUser);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<User>(deleteUser));
            }
            catch (DbUpdateException )
            {
                return StatusCode(415, new ResultViewModel<User>("GHHY - 9 Falha ao excluir o usuario, verifique o Id inserido"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<User>("F-5 Falha ao excluir o usuario"));
            }
        }
        
    }

}
