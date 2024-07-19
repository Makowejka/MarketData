using FluentValidation;
using MarketData.Domain.Contract;
using MarketData.Domain.Messages;
using MarketData.Infrastructure.Repositories;
using MarketData.Worker.Contract;
using MarketData.Worker.Extensions;
using MarketData.Worker.Services;
using MarketData.Worker.Validators;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<IUploadMarketDataFileService, UploadMarketDataFileService>();
builder.Services.AddScoped<IMarketDataFilesRepository, MarketDataFilesRepository>();
builder.Services.AddScoped<IValidator<UploadMarketDataFileMessage>, UploadMarketDataFileMessageValidator>();

builder.Services.AddMassTransit(builder.Configuration);

var host = builder.Build();
host.Run();
