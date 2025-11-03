using System.ComponentModel.DataAnnotations;

namespace NspStore.Domain.Entities
    {
    /// <summary>
    /// Товар в интернет-магазине NSP Store.
    /// </summary>
    public class Product
        {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Артикул (SKU)")]
        [Required(ErrorMessage = "Введите артикул товара")]
        public string Sku { get; set; } = null!;

        [Display(Name = "Ссылка (Slug)")]
        public string Slug { get; set; } = null!;

        [Display(Name = "Название товара")]
        [Required(ErrorMessage = "Введите название товара")]
        public string Name { get; set; } = null!;

        [Display(Name = "Краткое описание")]
        public string? ShortDescription { get; set; }

        [Display(Name = "Полное описание")]
        public string? FullDescription { get; set; }

        [Display(Name = "Состав (HTML)")]
        public string? CompositionHtml { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        public ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();

        public ICollection<Price> Prices { get; set; } = new List<Price>();
        }
    }
