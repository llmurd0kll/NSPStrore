using System.ComponentModel.DataAnnotations;

namespace NspStore.Domain.Entities
    {
    /// <summary>
    /// Изображение товара (разные размеры).
    /// </summary>
    public class ProductImage
        {
        public int Id { get; set; }

        /// <summary>
        /// Путь к оригиналу.
        /// </summary>
        [Display(Name = "Оригинал")]
        public string OriginalUrl { get; set; } = string.Empty;

        /// <summary>
        /// Путь к миниатюре (например, 300x300).
        /// </summary>
        [Display(Name = "Миниатюра")]
        public string ThumbUrl { get; set; } = string.Empty;

        /// <summary>
        /// Путь к среднему размеру (например, 800x800).
        /// </summary>
        [Display(Name = "Среднее изображение")]
        public string MediumUrl { get; set; } = string.Empty;

        /// <summary>
        /// Порядок сортировки.
        /// </summary>
        [Display(Name = "Порядок сортировки")]
        public int SortOrder { get; set; }

        /// <summary>
        /// Внешний ключ на товар.
        /// </summary>
        [Display(Name = "Id товара")]
        public int ProductId { get; set; }

        /// <summary>
        /// Навигационное свойство: товар.
        /// </summary>
        public Product Product { get; set; } = null!;
        }
    }
