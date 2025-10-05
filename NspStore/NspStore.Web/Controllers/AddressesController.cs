using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NspStore.Domain.Entities;
using NspStore.Infrastructure.Identity;
using NspStore.Infrastructure.Persistence;
using NspStore.Web.ViewModels;

namespace NspStore.Web.Controllers
{
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _users;

        public AddressesController(AppDbContext db, UserManager<ApplicationUser> users)
        {
            _db = db; _users = users;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _users.GetUserAsync(User);
            var list = await _db.Addresses.Where(a => a.UserId == user!.Id).OrderByDescending(a => a.IsDefault).ToListAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() => View(new AddressVm());

        [HttpPost]
        public async Task<IActionResult> Create(AddressVm vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var user = await _users.GetUserAsync(User);
            var entity = new Address
            {
                UserId = user!.Id,
                Country = vm.Country,
                City = vm.City,
                Street = vm.Street,
                Apartment = vm.Apartment,
                PostalCode = vm.PostalCode,
                IsDefault = vm.IsDefault
            };
            if (vm.IsDefault)
            {
                var others = await _db.Addresses.Where(a => a.UserId == user.Id && a.IsDefault).ToListAsync();
                foreach (var a in others) a.IsDefault = false;
            }
            _db.Addresses.Add(entity);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _users.GetUserAsync(User);
            var a = await _db.Addresses.FirstOrDefaultAsync(x => x.Id == id && x.UserId == user!.Id);
            if (a == null) return NotFound();
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

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddressVm vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var user = await _users.GetUserAsync(User);
            var a = await _db.Addresses.FirstOrDefaultAsync(x => x.Id == id && x.UserId == user!.Id);
            if (a == null) return NotFound();

            a.Country = vm.Country;
            a.City = vm.City;
            a.Street = vm.Street;
            a.Apartment = vm.Apartment;
            a.PostalCode = vm.PostalCode;

            if (vm.IsDefault && !a.IsDefault)
            {
                var others = await _db.Addresses.Where(x => x.UserId == user!.Id && x.IsDefault).ToListAsync();
                foreach (var o in others) o.IsDefault = false;
                a.IsDefault = true;
            }
            else if (!vm.IsDefault && a.IsDefault)
            {
                a.IsDefault = false;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _users.GetUserAsync(User);
            var a = await _db.Addresses.FirstOrDefaultAsync(x => x.Id == id && x.UserId == user!.Id);
            if (a == null) return NotFound();
            _db.Remove(a);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
