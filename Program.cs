using TradesAPI.Hubs;
using TradesAPI.Services;
using MessagePack;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR()
    .AddMessagePackProtocol(options =>
    {
        options.SerializerOptions = MessagePackSerializerOptions.Standard
            .WithCompression(MessagePackCompression.Lz4Block)
            .WithSecurity(MessagePackSecurity.UntrustedData);
    });

//builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyHeader()
               .AllowAnyMethod()
               .SetIsOriginAllowed((host) => true)
               .AllowCredentials());
});

builder.Services.AddSingleton<ITradeGenerator, TradeGenerator>();
builder.Services.AddHostedService<TradeCleanupService>();

var app = builder.Build();

app.UseCors("AllowAll");

app.MapHub<TradeHub>("/tradehub");

app.Run();
