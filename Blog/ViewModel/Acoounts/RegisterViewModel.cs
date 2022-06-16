using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModel.Acoounts
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nome obrigatorio")]
        [StringLength(200, ErrorMessage = "Seu nome deve ter no máximo 200 letras")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email obrigatorio")]
        [EmailAddress(ErrorMessage = "Email invalido")]
        public string Email { get; set; }
    }
}
