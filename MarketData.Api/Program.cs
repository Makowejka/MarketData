using FluentValidation;
using MarketData.Api.ExceptionHandler;
using MarketData.Api.Extensions;
using MarketData.Api.Services;
using MarketData.Api.Validators;
using MarketData.Domain.Contract;
using MarketData.Domain.Dto;
using MarketData.Domain.Options;
using MarketData.Infrastructure.Data;
using MarketData.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMarketDataFileService, MarketDataFileService>();

builder.Services.AddScoped<IMarketDataService, MarketDataService>();

builder.Services.AddScoped<IValidator<GetMarketDataDto>, GetMarketDataDtoValidator>();

builder.Services.Configure<AssetsOptions>(builder.Configuration.GetSection(nameof(AssetsOptions)));

builder.Services.AddControllers().ConfigureApiBehaviorOptions(x => x.SuppressMapClientErrors = true);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

await app.MigrateDataBaseAsync();

app.Run();
