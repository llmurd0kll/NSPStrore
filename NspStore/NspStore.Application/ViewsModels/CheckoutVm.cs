using System.ComponentModel.DataAnnotations;
using NspStore.Application.Services;
using NspStore.Domain.Entities;

namespace NspStore.Application.ViewModels
    {
    public class CheckoutVm
        {
        [Required(ErrorMessage = "Укажите адрес доставки")]
        public int? SelectedAddressId { get; set; }

        public string? Comment { get; set; }

        public List<CartItemDto> Items { get; set; } = new();

        public List<Address> Addresses { get; set; } = new();

        public decimal Total => Items.Sum(i => i.Price * i.Qty);
        }
    }
