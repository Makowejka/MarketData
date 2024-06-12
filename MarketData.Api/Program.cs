using MarketData.Api.Extensions;
using MarketData.Api.Services;
using MarketData.Domain.Contract;
using MarketData.Infrastructure.Data;
using MarketData.Infrastructure.ExceptionHandler;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMarketDataFileService, MarketDataFileService>();

builder.Services.AddScoped<IMarketDataService, MarketDataService>();

builder.Services.AddControllers();

builder.Services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

await app.MigrateDataBaseAsync();

app.Run();
