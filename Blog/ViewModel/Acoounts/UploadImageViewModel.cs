using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModel.Acoounts
{
    public class UploadImageViewModel
    {
        [Required(ErrorMessage = "Imagem invalida")]
        public string Base64Image { get; set; }
    }
}
