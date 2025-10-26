using NspStore.Domain.Entities;

namespace NspStore.Application.Services
{
    public static class PriceHelper
    {
        /// <summary>
        /// Возвращает актуальную цену продукта.
        /// Берёт последнюю по EffectiveFrom запись из коллекции Prices.
        /// Если цен нет — возвращает 0.
        /// </summary>
        public static decimal GetCurrentPrice(Product product)
        {
            if (product.Prices == null || !product.Prices.Any())
                return 0m; // если цен нет вообще

            var now = DateTime.UtcNow;

            // Берём все цены, которые уже вступили в силу
            var validPrices = product.Prices
                .Where(p => p.EffectiveFrom <= now && (p.EffectiveTo == null || p.EffectiveTo >= now));

            // Если есть подходящие по датам — берём последнюю
            var current = validPrices
                .OrderByDescending(p => p.EffectiveFrom)
                .FirstOrDefault();

            if (current != null)
                return current.Value;

            // Если нет подходящих по датам — берём самую последнюю из всех
            var latest = product.Prices
                .OrderByDescending(p => p.EffectiveFrom)
                .FirstOrDefault();

            return latest?.Value ?? 0m;
        }
    }
}
