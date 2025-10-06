using Microsoft.AspNetCore.Identity;
using NspStore.Domain.Entities;

namespace NspStore.Infrastructure.Identity
{
    /// <summary>
    /// Пользователь NSP Store.
    /// Наследуется от IdentityUser и расширяется дополнительными полями и связями.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Полное имя пользователя.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Навигационное свойство: список заказов пользователя.
        /// Настраивается в Infrastructure.Configurations.OrderConfiguration.
        /// </summary>
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        /// <summary>
        /// Навигационное свойство: список адресов пользователя.
        /// Настраивается в Infrastructure.Configurations.AddressConfiguration.
        /// </summary>
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
