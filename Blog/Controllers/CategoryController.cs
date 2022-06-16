using Blog.DataContext;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModel;
using Blog.ViewModel.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        //Iniciando o CRUD

        //Read
        [HttpGet("v1/categories")] //v1 = verssão 1 dp meu app
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context, [FromServices] IMemoryCache cache)
        {
            User.IsInRole("admin");
            
            try
            {
                var categories = cache.GetOrCreate("CategoryCache", x =>
                {
                    x.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return GetCategories(context);
                });
             
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch 
            {

                return StatusCode(500,new ResultViewModel<List<Category>>("45V-$ Falha ao encontrar o servidor"));
            }
           
        }
        private List<Category> GetCategories(BlogDataContext context)
        {
            return context.Categories.ToList();
        }
        //Read
        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync( [FromRoute] int id, [FromServices] BlogDataContext context)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Usuario não encontrado"));

                return Ok(new ResultViewModel<Category>(category));
            }
            //Esse catch vai vim do minha classe ResultViewModel do metodo de adicioanar erro
            catch 
            {

                return StatusCode(500, new ResultViewModel<List<Category>>("Falha interna no servidor"));
            }
           
        }

        //Create
        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel model, [FromServices] BlogDataContext context)
        {
            //Esse return irá vir lá do meu metodo GetErros 
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErros()));
            try
            {
                var category = new Category()
                {
                    Name=model.Name,
                    Slug=model.Slug.ToLower(),
                };
                await context.Categories.AddAsync(category); //Esperar salvar
                await context.SaveChangesAsync();//Esperar salvar

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("X12-Y Falha ao tentar inserir dados na categoria"));
            }
            catch 
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha ao inserir dados"));
            }
        }


        //Update
        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] EditorCategoryViewModel model, [FromServices] BlogDataContext context)
        {
            try
            {
                var receivePut = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);   //Esperar Atualizar 
                if (receivePut == null)
                    return NotFound(new ResultViewModel<Category>("Conteudo não encontrado"));

                receivePut.Name = model.Name;
                receivePut.Slug = model.Slug;

                context.Categories.Update(receivePut);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(receivePut));
            }
            catch (DbUpdateException ex)
            {

                return StatusCode(500, new ResultViewModel<Category>("X1908-4 Falha ao atualizar a tabela "));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("X1454-1 Falha ao Atualizar"));
            }
        }

        //Delete
        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext context)
        {
            try
            {
                var receiveDelete = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);  //Esperar deletar
                if (receiveDelete == null)
                    return NotFound(new ResultViewModel<Category>("Conteudo não encontrado"));

                context.Categories.Remove(receiveDelete);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(receiveDelete));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(415, new ResultViewModel<Category>("SWE12-X Falaha ao escluir a categoria, verifique o Id inserido"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("D45T-F Falaha ao excluir um usuario "));
            }
        }

    }
}
