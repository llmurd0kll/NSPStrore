using System.ComponentModel.DataAnnotations;

namespace NspStore.Web.Areas.Admin.ViewModels
{
    public class ProductEditVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название товара.")]
        [StringLength(200, ErrorMessage = "Название слишком длинное.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Введите slug.")]
        [StringLength(200, ErrorMessage = "Slug слишком длинный.")]
        public string Slug { get; set; } = "";

        [Required(ErrorMessage = "Введите артикул (SKU).")]
        [StringLength(50, ErrorMessage = "SKU слишком длинный.")]
        public string Sku { get; set; } = "";

        [Range(0.01, 999999, ErrorMessage = "Цена должна быть больше 0.")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Краткое описание слишком длинное.")]
        public string ShortDescription { get; set; } = "";

        [Required(ErrorMessage = "Выберите категорию.")]
        public int CategoryId { get; set; }

        public List<(int Id, string Url)> Images { get; set; } = new();

        public IEnumerable<(int Id, string Name)> CategoryOptions { get; set; } = Enumerable.Empty<(int, string)>();
    }
}
