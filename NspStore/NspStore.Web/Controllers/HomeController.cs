using Microsoft.AspNetCore.Mvc;

namespace NspStore.Web.Controllers
{
    /// <summary>
    /// Главный контроллер приложения.
    /// Отвечает за отображение стартовой страницы.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Главная страница сайта.
        /// </summary>
        public IActionResult Index() => View();
    }
}
