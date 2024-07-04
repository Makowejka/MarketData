using MarketData.Worker.Contract;
using MarketData.Worker.Extensions;
using MarketData.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<IUploadMarketDataFileService, UploadMarketDataFileService>();

builder.Services.AddMassTransit(builder.Configuration);

var host = builder.Build();
host.Run();
