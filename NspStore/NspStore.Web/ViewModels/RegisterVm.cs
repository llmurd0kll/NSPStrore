using System.ComponentModel.DataAnnotations;

namespace NspStore.Web.ViewModels
{
    /// <summary>
    /// ViewModel для формы регистрации нового пользователя.
    /// Используется в AccountController (Register).
    /// </summary>
    public class RegisterVm
    {
        /// <summary>
        /// Email пользователя (используется как логин).
        /// </summary>
        [Required(ErrorMessage = "Введите email")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Полное имя пользователя.
        /// </summary>
        [Required(ErrorMessage = "Введите имя")]
        [MinLength(2, ErrorMessage = "Имя должно содержать минимум 2 символа")]
        [StringLength(100, ErrorMessage = "Имя слишком длинное")]
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(8, ErrorMessage = "Пароль должен содержать минимум 8 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        /// <summary>
        /// Подтверждение пароля.
        /// </summary>
        [Required(ErrorMessage = "Повторите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
