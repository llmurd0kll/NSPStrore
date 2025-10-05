using Microsoft.AspNetCore.Identity;

namespace NspStore.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        // Навигации на Address/Order не держим здесь, чтобы не тянуть домен в Identity
    }
}
