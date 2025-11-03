namespace NspStore.Application.ViewsModels
    {
    public class ProductDetailsVm
        {
        public int Id { get; set; }
        public string Slug { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public string? CompositionHtml { get; set; }
        public string Sku { get; set; } = null!;

        public decimal CurrentPrice { get; set; }
        public decimal PartnerPrice { get; set; }

        public List<ProductImageVm> Images { get; set; } = new();

        public string MainImage =>
            Images.OrderBy(i => i.SortOrder).FirstOrDefault()?.OriginalUrl
            ?? "/images/placeholder.png";
        }
    }
