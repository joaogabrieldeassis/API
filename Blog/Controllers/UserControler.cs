using Blog.DataContext;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModel;
using Blog.ViewModel.Acoounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers
{
    public class UserControler : ControllerBase
    {
        [HttpPost("v1/accounts/")]
        public async Task<IActionResult> Post([FromBody] RegisterViewModel model, [FromServices] EmailServices email, [FromServices] BlogDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace("@", "-").Replace(".", "-")
            };
            var receivePassword = PasswordGenerator.Generate(25, includeSpecialChars: true, upperCase: true);
            user.PassWordHash = PasswordHasher.Hash(receivePassword);
            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                email.Send(
                    user.Name,
                    user.Email,
                    $"Olá {user.Name} :)",
                    $"Sua senha provissoria é:\n {receivePassword}"
                    );

                return Ok(new ResultViewModel<dynamic>(new
                {
                    user = user.Email
                }));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("456_X - Este Email já esta cadastrado"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("456-D Falha interna ao cadastrar o usuario"));
            }
        }

        [HttpPost("v1/accounts/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModels model, [FromServices] BlogDataContext context, [FromServices] TokenServices tokenServices)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

            var user = await context
                .Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == model.Email);
            //Verificando se o usuario existe
            if (user == null)
                return StatusCode(401, new ResultViewModel<string>("Usuario ou senha invalidor"));

            //Verificando se a senha é valida
            if (!PasswordHasher.Verify(user.PassWordHash, model.Password))
                return StatusCode(401, new ResultViewModel<string>("Usuario ou senha invalidos"));

            try
            {
                var token = tokenServices.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, null));
            }
            catch
            {

                return StatusCode(500, new ResultViewModel<string>("95qR - Falha interna no servidor"));
            }
        }
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
