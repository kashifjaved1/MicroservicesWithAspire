using System.Text.Json.Serialization;
using Auth.API.Authorization;
using Auth.API.Entities;
using Auth.API.Extensions;
using Auth.API.Helpers;
using Auth.API.Services;
using Grpc.Net.Client;
using MicroservicesWithAspire.ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ServiceDiscovery.Http;
using UrlShortener.API;

//using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddGrpc();
builder.Services.AddGrpcClient<Greeter.GreeterClient>(options =>
{
    options.Address = new Uri("https://urlshortenerapi");
});

// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;

    services.AddDbContext<DataContext>();
    services.AddCors();

    services.AddControllers().AddJsonOptions(x =>
        {
            // Serialize enums as strings in api responses (e.g. Role)
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

            // Hide values from Json when they are null
            x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

            // Make sure that the Json data are more easy to read / pretty print
            x.JsonSerializerOptions.WriteIndented = true;

            // Note: To prevent the JsonException thrown when try to get a List of 
            // all Accounts with all their individual RefreshTokens:
            // "JsonException: A possible object cycle was detected"
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        }
    );

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configure DI for application services
    services.AddScoped<IJwtUtils, JwtUtils>();
    services.AddScoped<IUserService, UserService>();

    builder.Services.AddCustomSwagger();
}

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/greeter", (Greeter.GreeterClient client) =>
{
    try
    {
        var reply = client.SayHello(new HelloRequest { Name = "World" });
    }
    catch (Exception)
    {
        throw;
    }
});

// Note: Make sure there are files created for Migration before using the program:
// Open Powershell and execute these commands:
//set ASPNETCORE_ENVIRONMENT=Development
//dotnet ef migrations add InitialCreate --context DataContext --output-dir Migrations/SqliteMigrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();

    // The DB will be deleted on every "dotnet run" / startup
    // If only used locally inMemory DB could be used
    context.Database.EnsureDeleted();

    // Make a DB Migration
    context.Database.Migrate();

    // Add hardcoded test and admin user to db on startup
    // Add a User - Test  
    var testUser = new User
    {
        FirstName = "Test",
        LastName = "User - 02-09-2025",
        Username = "test",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("test")
    };
    context.Users.Add(testUser);

    // Add a User - Admin  
    var testAdmin = new User
    {
        FirstName = "Admin",
        LastName = "User - 02-09-2025",
        Username = "admin",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin")
    };
    context.Users.Add(testAdmin);

    // Save the Users to the DB
    context.SaveChanges();

}

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}

app.UseCustomSwagger();

app.Run();