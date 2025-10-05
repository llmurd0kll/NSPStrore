using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NspStore.Application.Interfaces;
using NspStore.Application.Services;
using NspStore.Infrastructure.Identity;
using NspStore.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opts => {
    opts.Password.RequiredLength = 8;
    opts.Password.RequireDigit = true;
    opts.Password.RequireUppercase = false;
    opts.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization(opts => {
    opts.AddPolicy("RequireAdmin", p => p.RequireRole("Admin"));
    opts.AddPolicy("RequireManager", p => p.RequireRole("Manager", "Admin"));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddScoped<ICatalogService, CatalogService>();

builder.Services.ConfigureApplicationCookie(opts => {
    opts.LoginPath = "/Account/Login";
    opts.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await IdentitySeed.RunAsync(scope.ServiceProvider);
    await DataSeed.RunAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public static class IdentitySeed
{
    public static async Task RunAsync(IServiceProvider sp)
    {
        var roleMgr = sp.GetRequiredService<RoleManager<IdentityRole>>();
        var userMgr = sp.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { "Admin", "Manager", "Customer" })
        {
            if (!await roleMgr.RoleExistsAsync(role))
                await roleMgr.CreateAsync(new IdentityRole(role));
        }

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

