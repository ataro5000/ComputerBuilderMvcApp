using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ComputerBuilderMvcApp.Data;
using ComputerBuilderMvcApp.Models;
using ComputerBuilderMvcApp.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ComputerBuilderMvcApp.Data.ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<Customer>(options =>
options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ComputerBuilderMvcApp.Data.ApplicationDbContext>();

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IComponentService, ComponentService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false; 
});

builder.Services.AddScoped(SessionCart.GetCart);


var app = builder.Build();

using (var scope = app.Services.CreateScope())

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


