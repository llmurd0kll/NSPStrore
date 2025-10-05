using Microsoft.AspNetCore.Mvc;

namespace NspStore.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
