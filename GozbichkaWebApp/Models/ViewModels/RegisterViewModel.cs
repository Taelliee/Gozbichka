using System.ComponentModel.DataAnnotations;

namespace GozbichkaWebApp.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Имейлът е задължителен.")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Потребителското име е задължително.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Потребителското име трябва да е между 3 и 50 символа.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Паролата трябва да е поне 6 символа.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Моля, повторете паролата.")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
        public string ConfirmPassword { get; set; }
    }
}