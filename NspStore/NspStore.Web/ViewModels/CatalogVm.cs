using NspStore.Domain.Entities;

namespace NspStore.Web.ViewModels
{
    public class CatalogVm
    {
        public IReadOnlyList<Product> Items { get; set; } = Array.Empty<Product>();
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; } = 12;
        public string? Q { get; set; }
        public string? Category { get; set; }
    }
}
