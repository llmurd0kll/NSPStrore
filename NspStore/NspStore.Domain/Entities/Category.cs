using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NspStore.Domain.Entities
{
    /// <summary>
    /// Категория товаров в интернет-магазине NSP Store.
    /// Используется для группировки продуктов (например: "БАДы", "Косметика").
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Уникальный идентификатор категории (PK).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ЧПУ-строка (slug) для категории.
        /// Используется в URL (например: /catalog/vitamins).
        /// </summary>
        [Display(Name = "Ссылка в url например: /catalog/vitamins)")]
        public string Slug { get; set; } = null!;

        /// <summary>
        /// Название категории (отображается пользователю).
        /// </summary>
        [Display(Name = "Название категории")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Описание категории (опционально).
        /// Может использоваться для SEO или пояснения пользователю.
        /// </summary>
        [Display(Name = "Описание")]
        public string? Description { get; set; }

        /// <summary>
        /// Навигационное свойство: список продуктов,
        /// которые относятся к данной категории.
        /// </summary>
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
