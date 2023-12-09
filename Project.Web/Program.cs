using Microsoft.EntityFrameworkCore;
using Project.Core.Config;
using Project.Infrasturcture.Data;
using Project.Web.Extensions;
using Project.Web.Hubs;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
var cookieName = builder.Configuration.GetValue<string>("AppSettings:Cookie:Name");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection"));
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
builder.Services.AddSingleton<SignalRUserMappingService>();
builder.Services.AddSingleton<SignalRHubService>();
builder.Services.AddSignalR();
#region New
builder.Services.AddSession();
builder.Services.RegisterService();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseSession();  // Add this line before other middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages(); //  Is called to enable Razor Pages which sets up Razor Pages in the application

// Enable API Explorer middleware
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
    endpoints.MapHub<SignalRHub>("/signalRHub");
});

//app.MapControllers(); // Map your controllers
app.Run();
