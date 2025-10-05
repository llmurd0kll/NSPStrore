using NspStore.Domain.Enums;

namespace NspStore.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = null!; // FK на AspNetUsers
        public OrderStatus Status { get; set; } = OrderStatus.New;
        public decimal Total { get; set; }
        public string? Comment { get; set; }
        public int? ShippingAddressId { get; set; }
        public Address? ShippingAddress { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
