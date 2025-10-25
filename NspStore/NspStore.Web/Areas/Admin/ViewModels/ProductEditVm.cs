using NspStore.Web.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace NspStore.Web.Areas.Admin.ViewModels
{
    public class ProductEditVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название товара.")]
        public string Name { get; set; } = "";

        public string Slug { get; set; } = "";
        public string Sku { get; set; } = "";
        public decimal Price { get; set; }
        public string ShortDescription { get; set; } = "";
        public int CategoryId { get; set; }

        public List<ProductImageVm> Images { get; set; } = new();
        public IEnumerable<CategoryOptionVm> CategoryOptions { get; set; } = new List<CategoryOptionVm>();
    }
}
