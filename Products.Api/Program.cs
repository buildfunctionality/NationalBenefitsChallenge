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
using Npgsql;
using Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Cache")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Cache"));

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting up");

// Configure Circuit Breaker Policy
var circuitBreakerPolicy = Policy
    .Handle<PostgresException>()
    .CircuitBreakerAsync(
        exceptionsAllowedBeforeBreaking: 3,
        durationOfBreak: TimeSpan.FromSeconds(30),
        onBreak: (ex, timespan) =>
        {
            Log.Error("Circuit Breaker Tripped! DB is unavailable.");

            Console.WriteLine("Circuit Breaker Tripped! DB is unavailable.");
        },
        onReset: () =>
        {
            Log.Warning("Circuit Breaker Reset! DB is back online.");
            Console.WriteLine("Circuit Breaker Reset! DB is back online.");
        });

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();

builder.Services.AddScoped<Logger>();


builder.Services.AddScoped<IProductService,ProductService>(); 
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>(); 


builder.Services.AddScoped<IProductServiceCached, CachedProductService>();
builder.Services.AddScoped<ISubCategoryServiceCached, CachedSubCategoryService>();



var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context
        => await Results.Problem()
                     .ExecuteAsync(context)));
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapProductsEndpoints();
app.MapSubCategoryEndpoints();
app.Run();
