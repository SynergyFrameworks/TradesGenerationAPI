using TradesAPI.Services;
using Microsoft.AspNetCore.SignalR;

namespace TradesAPI.Hubs;

public class TradeHub : Hub
{
    private readonly ITradeGenerator _tradeGenerator;

    public TradeHub(ITradeGenerator tradeGenerator)
    {
        _tradeGenerator = tradeGenerator;
    }

    public override async Task OnConnectedAsync()
    {
        await _tradeGenerator.StartGeneratingTrades(Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _tradeGenerator.StopGeneratingTrades(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}