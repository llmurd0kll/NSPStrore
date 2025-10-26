using NspStore.Application.ViewsModels;

namespace NspStore.Web.Areas.Admin.ViewModels
{
    public class ProductEditVm
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Sku { get; set; }
        public string? ShortDescription { get; set; }
        public int CategoryId { get; set; }

        public List<ProductImageVm> Images { get; set; } = new();

        // Прайс-часть
        public decimal? CurrentPrice { get; set; } // только для отображения
        public decimal? NewPrice { get; set; }     // админ вводит новую цену
        public string Currency { get; set; } = "BYN";

        public List<CategoryOptionVm> CategoryOptions { get; set; } = new();
    }
}
