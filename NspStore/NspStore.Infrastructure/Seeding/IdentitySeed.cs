using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NspStore.Infrastructure.Identity;

namespace NspStore.Infrastructure.Seeding
    {
    public static class IdentitySeed
        {
        public static async Task RunAsync(IServiceProvider sp)
            {
            var roleMgr = sp.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = sp.GetRequiredService<UserManager<ApplicationUser>>();

            // Создание ролей
            foreach (var role in new[] { "Admin", "Manager", "Customer" })
                {
                if (!await roleMgr.RoleExistsAsync(role))
                    await roleMgr.CreateAsync(new IdentityRole(role));
                }

            // Создание администратора
            var adminEmail = "admin@nsp.local";
            var admin = await userMgr.FindByEmailAsync(adminEmail);
            if (admin == null)
                {
                admin = new ApplicationUser
                    {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "Site Admin"
                    };
                await userMgr.CreateAsync(admin, "Admin12345!");
                await userMgr.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
