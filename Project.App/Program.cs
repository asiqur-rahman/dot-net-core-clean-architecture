using Microsoft.EntityFrameworkCore;
using Project.App.Extensions;
using Project.App.Hubs;
using Project.Core.Config;
using Project.Infrasturcture.Data;

var builder = WebApplication.CreateBuilder(args);
var cookieName = builder.Configuration.GetValue<string>("AppSettings:Cookie:Name");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainMsSqlConnection"));
});

builder.Services.AddDbContext<MessagingDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MessagingMsSqlConnection"));
});

// Add services to the container.
builder.Services.AddControllersWithViews(); // is used for controllers that serve both API endpoints and HTML views. This allows you to generate HTML content in addition to handling API requests.

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = cookieName;
    options.DefaultSignInScheme = cookieName;
    options.DefaultChallengeScheme = cookieName;
    options.DefaultForbidScheme = cookieName;
})
.AddCookie(cookieName, options =>
{
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromSeconds(builder.Configuration.GetValue<double>("", 18000));
    options.LoginPath = new PathString("/Account/Login");
    options.LogoutPath = new PathString("/Account/Logout");
    options.AccessDeniedPath = new PathString("/Account/Forbidden");
    options.ReturnUrlParameter = "returnUrl";
    options.Cookie.SameSite = SameSiteMode.Strict;
    //options.ClaimsIssuer = "https://localhost:44381/";
    options.Cookie = new CookieBuilder
    {
        Name = cookieName,
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        IsEssential = true,
        SecurePolicy = CookieSecurePolicy.Always
    };
});

builder.Services.AddRazorPages();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddSignalR();
builder.Services.AddSession();
builder.Services.RegisterService();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();  // Add this line before other middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Configuring other endpoints, such as controllers and API endpoints
    endpoints.MapRazorPages(); // Specifically configures how requests to those Razor Pages are handled within the endpoint routing system
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Dashboard}/{id?}"); //Configures the routing
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Dashboard}/{id?}"); //Configures the routing
    endpoints.MapHub<SignalRHub>("/signalRHub", options =>
    {
        options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
    });
});

app.Run();
