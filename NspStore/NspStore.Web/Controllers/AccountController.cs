using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Infrastructure.Identity;
using NspStore.Infrastructure.Persistence;
using NspStore.Web.ViewModels;

namespace NspStore.Web.Controllers
{
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

        // --- Регистрация и логин оставляем как есть ---
        [HttpGet, AllowAnonymous]
        public IActionResult Register() => View(new RegisterVm());

        [HttpPost, AllowAnonymous]
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
                await _users.AddToRoleAsync(user, "Customer");
                await _signIn.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var e in result.Errors)
                ModelState.AddModelError(string.Empty, e.Description);

            return View(vm);
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login(string? returnUrl = null) =>
            View(new LoginVm { ReturnUrl = returnUrl });

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _signIn.PasswordSignInAsync(
                vm.Email, vm.Password, vm.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
                return Redirect(vm.ReturnUrl ?? Url.Action("Index", "Home")!);

            ModelState.AddModelError(string.Empty, "Неверная почта или пароль");
            return View(vm);
        }

        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() => View();

        // --- Новые действия ---

        // Профиль
        public async Task<IActionResult> Profile()
        {
            var user = await _users.GetUserAsync(User);
            return View(user);
        }

        // Мои заказы
        public async Task<IActionResult> Orders()
        {
            var user = await _users.GetUserAsync(User);
            var orders = await _db.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == user!.Id)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }
    }
}
