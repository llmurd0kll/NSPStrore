using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Application.ViewsModels;
using NspStore.Infrastructure.Identity;
using NspStore.Infrastructure.Persistence;

namespace NspStore.Web.Controllers
{
    /// <summary>
    /// Контроллер для управления аккаунтом пользователя:
    /// регистрация, вход, выход, профиль и заказы.
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _users;
        private readonly SignInManager<ApplicationUser> _signIn;
        private readonly AppDbContext _db;

        public AccountController(
            UserManager<ApplicationUser> users,
            SignInManager<ApplicationUser> signIn,
            AppDbContext db)
        {
            _users = users;
            _signIn = signIn;
            _db = db;
        }

        // --- Регистрация ---
        [HttpGet, AllowAnonymous]
        public IActionResult Register() => View(new RegisterVm());

        [HttpPost, AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = new ApplicationUser
            {
                UserName = vm.Email,
                Email = vm.Email,
                FullName = vm.FullName
            };

            var result = await _users.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                // Добавляем роль Customer (роль должна существовать в БД)
                await _users.AddToRoleAsync(user, "Customer");
                await _signIn.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var e in result.Errors)
                ModelState.AddModelError(string.Empty, e.Description);

            return View(vm);
        }

        // --- Логин ---
        [HttpGet, AllowAnonymous]
        public IActionResult Login(string? returnUrl = null) =>
            View(new LoginVm { ReturnUrl = returnUrl });

        [HttpPost, AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signIn.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _users.FindByEmailAsync(model.Email);
                var roles = await _users.GetRolesAsync(user);

                // Если админ или менеджер → сразу в админку
                if (roles.Contains("Admin") || roles.Contains("Manager"))
                {
                    return RedirectToAction("Index", "Products", new { area = "Admin" });
                }

                // иначе обычный редирект
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "Неверный логин или пароль");
            return View(model);
        }

        // --- Выход ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() => View();

        // --- Профиль ---
        public async Task<IActionResult> Profile()
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            // Для диплома лучше сделать отдельный ProfileVm
            return View(user);
        }

        // --- Мои заказы ---
        public async Task<IActionResult> Orders()
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var orders = await _db.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        // --- Вспомогательный метод ---
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl)
                && Url.IsLocalUrl(returnUrl)
                && !returnUrl.Contains("/Account/Login", StringComparison.OrdinalIgnoreCase))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
