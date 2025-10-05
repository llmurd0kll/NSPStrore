using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NspStore.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public string? CompositionHtml { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
    }
}
