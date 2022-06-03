using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Extensions
{
    public static class ModelStateExtensions
    {
        //Metodo statico para adicionar e retornar a lista de erro que criei no meu EditorCategoryViewModel
        public static List<string> GetErros(this ModelStateDictionary modelState)
        {
            var result = new List<string>();    
            foreach (var item in modelState.Values)
            {
                foreach (var item2 in item.Errors)
                {
                    result.Add(item2.ErrorMessage); 
                }
            }
            return result;
        }
    }
}
