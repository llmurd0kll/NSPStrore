using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NspStore.Application.Interfaces;
using NspStore.Application.Services;
using NspStore.Infrastructure.Data;
using NspStore.Infrastructure.Identity;
using NspStore.Infrastructure.Persistence;
using NspStore.Web.Services;

var builder = WebApplication.CreateBuilder(args);
// Хелпер для преобразования DATABASE_URL → Npgsql connection string
string BuildConnectionString(WebApplicationBuilder builder)
{
    // 1. Пробуем взять DATABASE_URL (Railway/Heroku стиль)
    var rawUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (!string.IsNullOrEmpty(rawUrl))
    {
        var uri = new Uri(rawUrl);
        var userInfo = uri.UserInfo.Split(':');

        var host = uri.Host;
        var port = uri.Port > 0 ? uri.Port : 5432;
        var db = uri.AbsolutePath.TrimStart('/');
        var user = userInfo[0];
        var pass = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";

        return $"Host={host};Port={port};Database={db};Username={user};Password={pass};SSL Mode=Require;Trust Server Certificate=true;";
    }

    // 2. Если DATABASE_URL нет → fallback на appsettings.json
    return builder.Configuration.GetConnectionString("DefaultConnection");
}

// Собираем строку подключения
var connectionString = BuildConnectionString(builder);

// 1. MVC
builder.Services.AddControllersWithViews();

// 2. EF Core + Postgress
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


// 3. Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opts => {
    opts.Password.RequiredLength = 8;
    opts.Password.RequireDigit = true;
    opts.Password.RequireUppercase = false;
    opts.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// 4. Authorization
builder.Services.AddAuthorization(opts => {
    opts.AddPolicy("RequireAdmin", p => p.RequireRole("Admin"));
    opts.AddPolicy("RequireManager", p => p.RequireRole("Manager", "Admin"));
});

// 5. Session + CartService
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddScoped<CartService>();

// 6. Application services
builder.Services.AddScoped<ICatalogService, CatalogService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.ConfigureApplicationCookie(opts => {
    opts.LoginPath = "/Account/Login";
    opts.AccessDeniedPath = "/Account/AccessDenied";
});

// Если переменная ASPNETCORE_URLS задана (например, в Docker), она перекроет launchSettings.json
builder.WebHost.UseUrls(builder.Configuration["ASPNETCORE_URLS"] ?? "http://localhost:5000;https://localhost:5001");

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddScoped<CartService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await DbInitializer.SeedAsync(context, userManager, roleManager);
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

//app.MapGet("/", () => " Приложение запущено");

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

