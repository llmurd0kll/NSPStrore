using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Application.ViewsModels;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Identity;
using NspStore.Infrastructure.Persistence;

namespace NspStore.Web.Controllers
{
    /// <summary>
    /// Контроллер для управления адресами доставки пользователя.
    /// Позволяет просматривать, добавлять, редактировать и удалять адреса.
    /// </summary>
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _users;

        public AddressesController(AppDbContext db, UserManager<ApplicationUser> users)
        {
            _db = db;
            _users = users;
        }

        /// <summary>
        /// Список адресов текущего пользователя.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var list = await _db.Addresses
                .Where(a => a.UserId == user.Id)
                .OrderByDescending(a => a.IsDefault)
                .ToListAsync();

            return View(list);
        }

        /// <summary>
        /// Форма добавления нового адреса.
        /// </summary>
        [HttpGet]
        public IActionResult Create() => View(new AddressVm());

        /// <summary>
        /// Обработка добавления нового адреса.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(AddressVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _users.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var entity = new Address
            {
                UserId = user.Id,
                Country = vm.Country,
                City = vm.City,
                Street = vm.Street,
                Apartment = vm.Apartment,
                PostalCode = vm.PostalCode,
                IsDefault = vm.IsDefault
            };

            if (vm.IsDefault)
                await ResetDefaultAddresses(user.Id);

            _db.Addresses.Add(entity);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Форма редактирования адреса.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var a = await _db.Addresses.FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);
            if (a == null) return RedirectToAction(nameof(Index));

            return View(new AddressVm
            {
                Country = a.Country,
                City = a.City,
                Street = a.Street,
                Apartment = a.Apartment,
                PostalCode = a.PostalCode,
                IsDefault = a.IsDefault
            });
        }

        /// <summary>
        /// Обработка редактирования адреса.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddressVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _users.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var a = await _db.Addresses.FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);
            if (a == null) return RedirectToAction(nameof(Index));

            a.Country = vm.Country;
            a.City = vm.City;
            a.Street = vm.Street;
            a.Apartment = vm.Apartment;
            a.PostalCode = vm.PostalCode;

            if (vm.IsDefault && !a.IsDefault)
            {
                await ResetDefaultAddresses(user.Id);
                a.IsDefault = true;
            }
            else if (!vm.IsDefault && a.IsDefault)
            {
                a.IsDefault = false;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Удаление адреса.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var a = await _db.Addresses.FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);
            if (a == null) return RedirectToAction(nameof(Index));

            _db.Remove(a);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Сбрасывает все адреса пользователя, помеченные как IsDefault.
        /// </summary>
        private async Task ResetDefaultAddresses(string userId)
        {
            var others = await _db.Addresses.Where(a => a.UserId == userId && a.IsDefault).ToListAsync();
            foreach (var o in others) o.IsDefault = false;
        }
    }
}
