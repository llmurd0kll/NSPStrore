using System.ComponentModel.DataAnnotations;

namespace NspStore.Web.ViewModels
{
    public class AddressVm
    {
        [Required] public string Country { get; set; } = "Беларусь";
        [Required] public string City { get; set; } = null!;
        [Required] public string Street { get; set; } = null!;
        public string? Apartment { get; set; }
        [Required] public string PostalCode { get; set; } = null!;
        public bool IsDefault { get; set; }
    }
}
