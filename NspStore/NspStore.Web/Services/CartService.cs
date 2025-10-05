using System.Text.Json;
using NspStore.Domain.Entities;

namespace NspStore.Web.Services
{
    /// <summary>
    /// DTO для хранения элемента корзины в сессии.
    /// </summary>
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Qty { get; set; }
    }

    /// <summary>
    /// Сервис для управления корзиной покупателя.
    /// Хранит данные в сессии пользователя (JSON).
    /// </summary>
    public class CartService
    {
        private readonly ISession _session;
        private const string Key = "cart";

        public CartService(IHttpContextAccessor http)
        {
            if (http.HttpContext == null)
                throw new ArgumentNullException(nameof(http.HttpContext), "HttpContext недоступен");

            _session = http.HttpContext.Session;
        }

        /// <summary>
        /// Получить содержимое корзины из сессии.
        /// </summary>
        public List<CartItemDto> Get()
        {
            var json = _session.GetString(Key);
            return json == null
                ? new List<CartItemDto>()
                : JsonSerializer.Deserialize<List<CartItemDto>>(json)!;
        }

        /// <summary>
        /// Сохранить корзину в сессии.
        /// </summary>
        public void Save(List<CartItemDto> items)
        {
            var json = JsonSerializer.Serialize(items);
            _session.SetString(Key, json);
        }

        /// <summary>
        /// Добавить товар в корзину.
        /// Если товар уже есть — увеличивает количество.
        /// </summary>
        public void Add(Product p, int qty = 1)
        {
            if (qty <= 0) return; // защита от некорректного количества

            var items = Get();
            var item = items.FirstOrDefault(i => i.ProductId == p.Id);

            if (item == null)
            {
                items.Add(new CartItemDto
                {
                    ProductId = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Qty = qty
                });
            }
            else
            {
                item.Qty += qty;
            }

            Save(items);
        }

        /// <summary>
        /// Удалить товар из корзины по его Id.
        /// </summary>
        public void Remove(int productId)
        {
            var items = Get();
            items.RemoveAll(i => i.ProductId == productId);
            Save(items);
        }

        /// <summary>
        /// Очистить корзину.
        /// </summary>
        public void Clear() => Save(new List<CartItemDto>());
    }
}
