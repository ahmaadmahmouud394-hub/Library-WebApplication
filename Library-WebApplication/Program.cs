using Library_WebApplication.Busniness_Object;
using Library_WebApplication.Middleware;
using Library_WebApplication.Models;
using Library_WebApplication.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAuthorization(options =>
{
    // Admin only
    options.AddPolicy("RequireAdmin", policy =>
        policy.RequireRole("Admin"));

    // Employee only
    options.AddPolicy("RequireEmployee", policy =>
        policy.RequireRole("Employee"));

    // Client only
    options.AddPolicy("RequireClient", policy =>
        policy.RequireRole("Client"));

    // Example: Admin OR Employee can access
    options.AddPolicy("RequireAdminOrEmployee", policy =>
        policy.RequireRole("Admin", "Employee"));
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
    options.Cookie.IsEssential = true; // Mark the session cookie as essential
});

// ✅ MVC Controllers
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/User/Login";
        o.AccessDeniedPath = "/User/AccessDenied";
        o.SlidingExpiration = true;
        o.ExpireTimeSpan = TimeSpan.FromDays(14);
        o.Cookie.HttpOnly = true;
        o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Always in prod https
    });

// ✅ Dependency Injection
builder.Services.AddScoped<AuthenticationBO>();
builder.Services.AddScoped<BooksBO>();
builder.Services.AddScoped<UserBO>();
builder.Services.AddScoped<Encryption>();
builder.Services.AddScoped<EmailValidation>();
builder.Services.AddScoped<TipologyBO>();
builder.Services.AddScoped<InvoicesBO>();
builder.Services.AddScoped<PubblisherBO>();
builder.Services.AddScoped<AuthorBO>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddAuthorization();

var app = builder.Build();

// ✅ Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseMiddleware<SessionValidationMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

// ✅ Route Configurations
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=User}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
