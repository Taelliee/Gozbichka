using System.ComponentModel.DataAnnotations;

namespace GozbichkaWebApp.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Потребителското име е задължително.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}