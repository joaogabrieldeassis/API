using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModel.Categories
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [StringLength(80, MinimumLength = 4, ErrorMessage = "O campo deve conter no minimo 4 caracter e no maximo 80")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [StringLength(80, MinimumLength = 4, ErrorMessage = "O campo deve conter no minimo 4 caracter e no maximo 80")]
        public string Slug { get; set; }
    }
}
