using System.Text.Json;
using NspStore.Domain.Entities;

namespace NspStore.Web.Services
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Qty { get; set; }
    }

    public class CartService
    {
        private readonly ISession _session;
        private const string Key = "cart";

        public CartService(IHttpContextAccessor http)
        {
            _session = http.HttpContext!.Session;
        }

        public List<CartItemDto> Get()
        {
            var json = _session.GetString(Key);
            return json == null ? new List<CartItemDto>() :
                JsonSerializer.Deserialize<List<CartItemDto>>(json)!;
        }

        public void Save(List<CartItemDto> items)
        {
            var json = JsonSerializer.Serialize(items);
            _session.SetString(Key, json);
        }

        public void Add(Product p, int qty = 1)
        {
            var items = Get();
            var item = items.FirstOrDefault(i => i.ProductId == p.Id);
            if (item == null)
            {
                items.Add(new CartItemDto { ProductId = p.Id, Name = p.Name, Price = p.Price, Qty = qty });
            }
            else
            {
                item.Qty += qty;
            }
            Save(items);
        }

        public void Remove(int productId)
        {
            var items = Get();
            items.RemoveAll(i => i.ProductId == productId);
            Save(items);
        }

        public void Clear() => Save(new List<CartItemDto>());
    }
}
