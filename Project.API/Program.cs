using Azure.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Routing;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Project.API.Middlewares;
using System.IO;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string routePrefix = builder.Configuration["AppSettings:ApiGlobalPrefix"];

#region Add services to the container

// The following code configures PascalCase formatting instead of the default camelCase formatting
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Clean Project API",
        Version = "v1.0",
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

#region JWT Service

//Configure JWT authentication at the time when the application starts. It specifies the authentication scheme as JwtBearer.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

//To add authorization services to your application
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

#endregion

#endregion

var app = builder.Build(); //After configuring services and middleware, you call builder.Build() to build and finalize the application's configuration. This method creates an IServiceProvider that includes all the registered services and middleware configurations. The IServiceProvider is essential for dependency injection and handling requests in your application.
                           //In summary, builder.Build() is an essential step to create the application's service provider and make all the configured services and middleware available to the application.

app.UseMiddleware<GlobalRoutePrefixMiddleware>(routePrefix); // By that the middleware will prepend "/api" to the beginning of all incoming request URLs.
app.UsePathBase(new PathString(routePrefix)); // By using app.UsePathBase(new PathString("/api")), essentially I am telling my application to prepend "/api" to the beginning of all route paths. This can be useful if I want to organize or version my API under a specific base path, such as "/api/v1," to distinguish it from other parts of my application.

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

#region To enable authentication and authorization capabilities
app.UseAuthentication();
app.UseAuthorization();
#endregion

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
