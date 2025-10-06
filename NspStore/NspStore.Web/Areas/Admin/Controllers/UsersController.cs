using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NspStore.Infrastructure.Identity;
using NspStore.Web.Areas.Admin.ViewModels;

namespace NspStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _users;

        public UsersController(UserManager<ApplicationUser> users)
        {
            _users = users;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            var users = _users.Users.ToList();
            var list = new List<UserVm>();

            foreach (var u in users)
            {
                var roles = await _users.GetRolesAsync(u);
                list.Add(new UserVm
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.FullName,
                    Roles = roles.ToList()
                });
            }

            return View(list);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _users.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _users.GetRolesAsync(user);
            ViewBag.Roles = roles;
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string[] roles)
        {
            var user = await _users.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentRoles = await _users.GetRolesAsync(user);

            // Убираем все текущие роли
            await _users.RemoveFromRolesAsync(user, currentRoles);

            // Добавляем выбранные роли
            if (roles != null && roles.Any())
            {
                await _users.AddToRolesAsync(user, roles);
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _users.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _users.FindByIdAsync(id);
            if (user != null)
            {
                await _users.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
