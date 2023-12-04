using Microsoft.EntityFrameworkCore;
using Project.Infrasturcture.Data;
using Project.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
var cookieName = builder.Configuration.GetValue<string>("Constants:CookieName", "CleanProject");

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

app.MapRazorPages(); // Configure Razor Pages

// Enable API Explorer middleware
app.UseEndpoints(endpoints => 
{
    endpoints.MapControllers(); // Map your controllers
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"); //Configures the routing
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"); //Configures the routing
});

//app.MapControllers(); // Map your controllers
app.Run();
