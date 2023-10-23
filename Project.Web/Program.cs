using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Project.Infrasturcture.Data;
using Project.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection"));
});

// Add services to the container.
builder.Services.AddControllersWithViews(); // is used for controllers that serve both API endpoints and HTML views. This allows you to generate HTML content in addition to handling API requests.

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

app.UseAuthorization();

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
