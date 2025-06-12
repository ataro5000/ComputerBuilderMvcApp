// This file is the main entry point for the ASP.NET Core application.
// It configures services, defines the HTTP request pipeline, and sets up routing.
// It also includes a static helper class `SessionCart` for managing the shopping cart in the session.
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ComputerBuilderMvcApp.Data;
using ComputerBuilderMvcApp.Models;
using ComputerBuilderMvcApp.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  
    options.UseSqlite(connectionString)); 

builder.Services.AddDefaultIdentity<Customer>(options => options.SignIn.RequireConfirmedAccount = true) 
    .AddEntityFrameworkStores<ApplicationDbContext>();
    
builder.Services.AddTransient<IEmailSender, EmailSender>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IComponentService, ComponentService>();
// Configure session services.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Sets session timeout.
    options.Cookie.HttpOnly = true; // Makes the session cookie inaccessible to client-side scripts.
    options.Cookie.IsEssential = true; // Marks the session cookie as essential for GDPR compliance.
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
    options.User.RequireUniqueEmail = false; // Set to true if you want unique emails
});

builder.Services.AddScoped(SessionCart.GetCart);


var app = builder.Build();

using (var scope = app.Services.CreateScope())


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Uses a generic error handler page in production.

    app.UseHsts(); // Adds HTTP Strict Transport Security Protocol (HSTS) headers.
}

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS.
app.UseStaticFiles(); // Enables serving static files (e.g., CSS, JavaScript, images).

app.UseRouting(); 

app.UseSession(); 

app.UseAuthentication();
app.UseAuthorization(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run(); 


