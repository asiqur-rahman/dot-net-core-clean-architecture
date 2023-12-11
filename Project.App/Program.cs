using Project.App.Hubs;
using Project.Core.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSignalR();

builder.Services.AddSingleton<List<StreamingUser>>();
builder.Services.AddSingleton<List<UserCall>>();
builder.Services.AddSingleton<List<CallOffer>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

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
    endpoints.MapHub<ConnectionHub>("/connectionHubs", options =>
    {
        options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
    });
});

app.Run();
