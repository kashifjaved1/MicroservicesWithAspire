using MicroservicesWithAspire.ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using Store.API.Controllers.Config;
using Store.API.Domain.Repositories;
using Store.API.Domain.Services;
using Store.API.Extensions;
using Store.API.Persistence.Contexts;
using Store.API.Persistence.Repositories;
using Store.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var configuration = builder.Configuration;

// --- Service registrations ---
builder.Services.AddMemoryCache();
builder.Services.AddCustomSwagger();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory.ProduceErrorResponse;
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase(configuration.GetConnectionString("memory") ?? "data-in-memory");
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddAutoMapper(typeof(Program));

// --- Build app ---
var app = builder.Build();

app.MapDefaultEndpoints();

// --- Middleware pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCustomSwagger();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

// --- Optional: seed database ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await SeedData.Seed(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Could not seed data.");
    }
}

app.UseCustomSwagger();

app.Run();