namespace NspStore.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!; // FK на AspNetUsers
        public string Country { get; set; } = "Беларусь";
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string? Apartment { get; set; }
        public string PostalCode { get; set; } = null!;
        public bool IsDefault { get; set; }
    }
}
