using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModel.Acoounts
{
    public class LoginViewModels
    {
        [Required(ErrorMessage = "Nome obrigatorio")]
        [StringLength(200, ErrorMessage = "Seu nome deve ter no máximo 200 letras")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha obrigatoria")]
        public string Password { get; set; }
    }
}
