using Microsoft.Extensions.Caching.Hybrid;
using Northwind.DataContext.Sqlite;
using Northwind.WebApi;
using Northwind.WebApi.Repositories;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "allowWasmClient",
        policy => { policy.WithOrigins("https://localhost:7104"); });
});
builder.Services.AddOpenApi();
builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromSeconds(60),
        LocalCacheExpiration = TimeSpan.FromSeconds(30)
    };
});
builder.Services.AddNorthwindContext();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpLogging();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapCustomers();

app.UseCors("allowWasmClient");

app.Run();
