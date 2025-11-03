using System.ComponentModel.DataAnnotations;

namespace NspStore.Application.ViewsModels
    {
    /// <summary>
    /// ViewModel для изображения товара.
    /// </summary>
    public class ProductImageVm
        {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Оригинал")]
        public string OriginalUrl { get; set; } = string.Empty;

        [Display(Name = "Миниатюра")]
        public string ThumbUrl { get; set; } = string.Empty;

        [Display(Name = "Среднее изображение")]
        public string MediumUrl { get; set; } = string.Empty;

        [Display(Name = "Порядок сортировки")]
        public int SortOrder { get; set; }
        }
    }
