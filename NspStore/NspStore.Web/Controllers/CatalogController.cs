using Microsoft.AspNetCore.Mvc;
using NspStore.Application.Interfaces;
using NspStore.Application.ViewsModels;

namespace NspStore.Web.Controllers
{
    /// <summary>
    /// Контроллер каталога.
    /// Отвечает за отображение списка товаров и страницы товара.
    /// </summary>
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalog;

        public CatalogController(ICatalogService catalog) => _catalog = catalog;

        /// <summary>
        /// Список товаров с поиском и пагинацией.
        /// </summary>
        /// <param name="q">Поисковый запрос (часть названия или описания).</param>
        /// <param name="category">Slug категории.</param>
        /// <param name="page">Номер страницы (по умолчанию 1).</param>
        public async Task<IActionResult> Index(string? q, string? category, int page = 1)
        {
            const int pageSize = 12;

            // сервис возвращает доменные сущности
            var (items, total) = await _catalog.SearchAsync(q, category, page, pageSize);

            // маппим Product -> ProductVm
            var vm = new CatalogVm
            {
                Items = items.Select(p => new ProductVm
                {
                    Id = p.Id,
                    Name = p.Name,
                    Slug = p.Slug,
                    CurrentPrice = p.CurrentPrice,
                    ShortDescription = p.ShortDescription,
                    Sku = p.Sku,
                    Images = p.Images.Select(img => new ProductImageVm
                    {
                        Url = img.Url,
                        SortOrder = img.SortOrder
                    }).ToList()
                }).ToList(),
                Total = total,
                Page = page,
                PageSize = pageSize,
                Q = q,
                Category = category
            };

            return View(vm);
        }


        /// <summary>
        /// Страница конкретного товара по slug.
        /// </summary>
        /// <param name="slug">Slug товара.</param>
        public async Task<IActionResult> Product(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return NotFound();

            var product = await _catalog.GetBySlugAsync(slug);
            if (product == null)
                return NotFound();

            // Для диплома лучше сделать ProductVm и маппить сюда
            return View(product);
        }
    }
}
