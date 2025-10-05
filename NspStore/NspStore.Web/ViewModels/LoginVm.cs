using System.ComponentModel.DataAnnotations;

namespace NspStore.Web.ViewModels
{
    /// <summary>
    /// ViewModel для формы входа пользователя.
    /// Используется в AccountController (Login).
    /// </summary>
    public class LoginVm
    {
        /// <summary>
        /// Email пользователя (используется как логин).
        /// </summary>
        [Required(ErrorMessage = "Введите email")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        /// <summary>
        /// Запомнить пользователя (persistent cookie).
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// URL для возврата после успешного входа.
        /// </summary>
        public string? ReturnUrl { get; set; }
    }
}
