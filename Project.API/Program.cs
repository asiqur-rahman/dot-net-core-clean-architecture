using Azure.Core;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;
using Project.API.Middlewares;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// The following code configures PascalCase formatting instead of the default camelCase formatting
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Clean Project API",
        Version = "v1.0",
    });
});

var app = builder.Build();

app.UseMiddleware<GlobalRoutePrefixMiddleware>("/api"); // By that the middleware will prepend "/api" to the beginning of all incoming request URLs.
app.UsePathBase(new PathString("/api")); // By using app.UsePathBase(new PathString("/api")), you are essentially telling your application to prepend "/api" to the beginning of all route paths. This can be useful if you want to organize or version your API under a specific base path, such as "/api/v1," to distinguish it from other parts of your application.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger"; // This sets the URL path for accessing Swagger UI
        c.DocumentTitle = "Clean Project API";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Project API");
    });
}


app.UseHttpsRedirection();


app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // Your other endpoint configurations
    endpoints.MapControllers();
    // Configure a custom route for Swagger UI
    endpoints.MapGet("/", async context =>
    {
        context.Response.Redirect("/swagger"); // Redirect to the default Swagger route
    });
});


app.Run();
