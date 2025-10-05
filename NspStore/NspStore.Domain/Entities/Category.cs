using System.Collections.Generic;

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
        public string Slug { get; set; } = null!;

        /// <summary>
        /// Название категории (отображается пользователю).
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Описание категории (опционально).
        /// Может использоваться для SEO или пояснения пользователю.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Навигационное свойство: список продуктов,
        /// которые относятся к данной категории.
        /// </summary>
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
