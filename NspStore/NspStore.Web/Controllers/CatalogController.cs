using Microsoft.AspNetCore.Mvc;
using NspStore.Application.Interfaces;
using NspStore.Web.ViewModels;

namespace NspStore.Web.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalog;
        public CatalogController(ICatalogService catalog) => _catalog = catalog;

        public async Task<IActionResult> Index(string? q, string? category, int page = 1)
        {
            const int pageSize = 12;
            var (items, total) = await _catalog.SearchAsync(q, category, page, pageSize);
            return View(new CatalogVm
            {
                Items = items,
                Total = total,
                Page = page,
                PageSize = pageSize,
                Q = q,
                Category = category
            });
        }

        public async Task<IActionResult> Product(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug)) return NotFound();
            var product = await _catalog.GetBySlugAsync(slug);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
