using System.ComponentModel.DataAnnotations;
using NspStore.Application.ViewsModels;

namespace NspStore.Web.Areas.Admin.ViewModels
    {
    public class ProductEditVm
        {
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название товара")]
        public string Name { get; set; } = null!;

        [Display(Name = "Slug (URL)")]
        [Required(ErrorMessage = "Введите slug")]
        public string Slug { get; set; } = null!;

        [Display(Name = "Артикул (SKU)")]
        [Required(ErrorMessage = "Введите артикул")]
        public string Sku { get; set; } = null!;

        [Display(Name = "Краткое описание")]
        public string? ShortDescription { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Выберите категорию")]
        public int CategoryId { get; set; }

        [Display(Name = "Текущая цена")]
        public decimal? CurrentPrice { get; set; }

        [Display(Name = "Новая цена")]
        public decimal? NewPrice { get; set; }

        /// <summary>
        /// Валюта для цены (по умолчанию BYN).
        /// </summary>
        [Display(Name = "Валюта")]
        public string Currency { get; set; } = "BYN";

        [Display(Name = "Изображения")]
        public List<ProductImageVm> Images { get; set; } = new();

        public List<CategorySelectVm> CategoryOptions { get; set; } = new();
        }
    }
