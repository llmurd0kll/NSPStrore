using System.ComponentModel.DataAnnotations;

namespace NspStore.Web.ViewModels
{
    public class RegisterVm
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(2)]
        public string FullName { get; set; } = null!;

        [Required, MinLength(8)]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
