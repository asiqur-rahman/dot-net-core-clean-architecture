using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Clean Project",
        Version = "v1.0",
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Project");
        c.RoutePrefix = "swagger"; // This sets the URL path for accessing Swagger UI
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
