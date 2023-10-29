using Microsoft.OpenApi.Models;
using Project.API.Middlewares;

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

//app.UseMiddleware<GlobalRoutePrefixMiddleware>("/api");
//app.UsePathBase(new PathString("/api"));
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger"; // This sets the URL path for accessing Swagger UI
        c.DocumentTitle = "Clean Project API";
        //c.InjectJavascript("/swagger-ui/rapipdf-min.js");
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
