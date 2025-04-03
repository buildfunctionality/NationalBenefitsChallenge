using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Products.Api.Database;
using Products.Api.Endpoints;
using Products.Api.Extensions;
using Products.Api.Services;
using Products.Api.Utils;
using StackExchange.Redis;
using Serilog;
using Products.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Cache")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Cache"));

// Configure logging
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();

//Log.Logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .CreateLogger();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting up");

//builder.Host.UseSerilog();  // Use Serilog for logging


//// Configure Circuit Breaker Policy
//var circuitBreakerPolicy = Policy
//    .Handle<SqlException>()
//    .CircuitBreakerAsync(
//        exceptionsAllowedBeforeBreaking: 3,
//        durationOfBreak: TimeSpan.FromSeconds(30),
//        onBreak: (ex, timespan) =>
//        {
//            Console.WriteLine("Circuit Breaker Tripped! DB is unavailable.");
//        },
//        onReset: () =>
//        {
//            Console.WriteLine("Circuit Breaker Reset! DB is back online.");
//        });

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();

builder.Services.AddScoped<Logger>();

// Register Services with Decorator Pattern
////builder.Services.AddTransient<IProductService, Pr>();
////builder.Services.Decorate<IProductService, CachedProductService>();
//builder.Services.AddScoped<IProductService, ProductService>();
// Register ProductService as the "inner" service (implementation of IProductService)
builder.Services.AddScoped<IProductService,ProductService>(); // Register the concrete class
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>(); // Register the concrete class

//builder.Services.AddScoped<ProductService>();
// Apply the Decorator AFTER the base service is registered
builder.Services.AddScoped<IProductServiceCached, CachedProductService>();
builder.Services.AddScoped<ISubCategoryServiceCached, CachedSubCategoryService>();



var app = builder.Build();
//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
   // app.ApplyMigrations();
}

app.MapProductsEndpoints();
app.MapSubCategoryEndpoints();
app.Run();
