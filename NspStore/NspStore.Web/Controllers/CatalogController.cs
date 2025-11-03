using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NspStore.Application.Interfaces;
using NspStore.Application.Options;
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
        private readonly IOptions<StoreOptions> _storeOptions;

        public CatalogController(ICatalogService catalog, IOptions<StoreOptions> storeOptions)
        {
            _catalog = catalog;
            _storeOptions = storeOptions;
        }

        /// <summary>
        /// Список товаров с поиском и пагинацией.
        /// </summary>
        public async Task<IActionResult> Index(string? q, string? category, int page = 1)
        {
            const int pageSize = 12;

            // товары
            var (items, total) = await _catalog.SearchAsync(q, category, page, pageSize);

            // категории
            var categories = await _catalog.GetCategoriesAsync();

            var vm = new CatalogVm
            {
                Items = items.Select(p => new ProductVm
                {
                    Id = p.Id,
                    Name = p.Name,
                    Slug = p.Slug,
                    CurrentPrice = _catalog.GetCurrentPrice(p),
                    ShortDescription = p.ShortDescription,
                    Sku = p.Sku,
                    Images = p.Images.Select(img => new ProductImageVm
                    {
                        Id = img.Id,
                        OriginalUrl = img.OriginalUrl,
                        ThumbUrl = img.ThumbUrl,
                        MediumUrl = img.MediumUrl,
                        SortOrder = img.SortOrder
                    }).ToList()
                }).ToList(),
                Categories = categories.Select(c => new CategoryVm
                {
                    Slug = c.Slug,
                    Name = c.Name
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
        public async Task<IActionResult> Product(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return NotFound();

            var product = await _catalog.GetBySlugAsync(slug);
            if (product == null)
                return NotFound();

            var currentPrice = _catalog.GetCurrentPrice(product);
            var discount = (decimal)_storeOptions.Value.Discount;

            var vm = new ProductDetailsVm
            {
                Id = product.Id,
                Slug = product.Slug,
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                FullDescription = product.FullDescription,
                CompositionHtml = product.CompositionHtml,
                Sku = product.Sku,
                CurrentPrice = currentPrice,
                PartnerPrice = Math.Round(currentPrice * (1 - discount), 2),
                Images = product.Images.Select(i => new ProductImageVm
                {
                    Id = i.Id,
                    OriginalUrl = i.OriginalUrl,
                    ThumbUrl = i.ThumbUrl,
                    MediumUrl = i.MediumUrl,
                    SortOrder = i.SortOrder
                }).ToList()
            };

            return View(vm);
        }
    }
}
