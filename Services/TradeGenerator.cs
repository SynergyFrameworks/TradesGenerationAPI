using TradesAPI.Hubs;
using TradesAPI.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace TradesAPI.Services;

public class TradeGenerator : ITradeGenerator
{
    private readonly IHubContext<TradeHub> _hubContext;
    private readonly ConcurrentDictionary<string, CancellationTokenSource> _cancellationTokens;
    private readonly ConcurrentQueue<OptionTrade> _historicalTrades;
    private readonly Random _random;
    private readonly ILogger<TradeGenerator> _logger;

    private readonly string[] Symbols = {
    // Original Tech Giants
    "AAPL",  // Apple
    "GOOGL", // Alphabet (Google)
    "MSFT",  // Microsoft
    "AMZN",  // Amazon
    "TSLA",  // Tesla
    
    // Technology & Communications
    "META",  // Meta (Facebook)
    "NVDA",  // NVIDIA
    "INTC",  // Intel
    "CSCO",  // Cisco Systems
    "ORCL",  // Oracle
    "IBM",   // IBM
    "QCOM",  // Qualcomm
    "AMD",   // Advanced Micro Devices
    
    // Financial Services
    "JPM",   // JPMorgan Chase
    "BAC",   // Bank of America
    "WFC",   // Wells Fargo
    "GS",    // Goldman Sachs
    "MS",    // Morgan Stanley
    "V",     // Visa
    "MA",    // Mastercard
    
    // Healthcare & Pharma
    "JNJ",   // Johnson & Johnson
    "UNH",   // UnitedHealth Group
    "PFE",   // Pfizer
    "MRK",   // Merck
    "ABBV",  // AbbVie
    
    // Retail & Consumer
    "WMT",   // Walmart
    "HD",    // Home Depot
    "TGT",   // Target
    "COST",  // Costco
    "NKE",   // Nike
    
    // Industrial & Manufacturing
    "CAT",   // Caterpillar
    "GE",    // General Electric
    "MMM",   // 3M
    "BA",    // Boeing
    "HON"    // Honeywell
};


    private const int MaxHistoricalTrades = 10000;

    public TradeGenerator(IHubContext<TradeHub> hubContext, ILogger<TradeGenerator> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
        _cancellationTokens = new ConcurrentDictionary<string, CancellationTokenSource>();
        _historicalTrades = new ConcurrentQueue<OptionTrade>();
        _random = new Random();
    }

    public async Task StartGeneratingTrades(string connectionId)
    {
        var cts = new CancellationTokenSource();
        _cancellationTokens.TryAdd(connectionId, cts);

        try
        {
            int tradeCount = 0;
            while (!cts.Token.IsCancellationRequested)
            {
                var trade = GenerateTrade();
                _historicalTrades.Enqueue(trade);
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveTrade", trade, cts.Token);
                await _hubContext.Clients.Group(trade.Symbol).SendAsync("ReceiveSymbolTrade", trade, cts.Token);

                _logger.LogInformation("Generated trade #{tradeCount}: {trade}", ++tradeCount, trade);

                await Task.Delay(_random.Next(100, 1000), cts.Token);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation($"Trade generation cancelled for client: {connectionId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error generating trades for client: {connectionId}");
        }
    }

    public void StopGeneratingTrades(string connectionId)
    {
        if (_cancellationTokens.TryRemove(connectionId, out var cts))
        {
            cts.Cancel();
            cts.Dispose();
        }
    }

    public IEnumerable<OptionTrade> GetHistoricalTrades(DateTime startDate, DateTime endDate)
    {
        return _historicalTrades
            .Where(t => t.Timestamp >= startDate && t.Timestamp <= endDate)
            .OrderByDescending(t => t.Timestamp);
    }

    public TradeStatistics CalculateStatistics(IEnumerable<OptionTrade> trades)
    {
        if (!trades.Any())
        {
            return new TradeStatistics
            {
                TotalVolume = 0,
                TotalValue = 0,
                AveragePrice = 0,
                IVSpread = 0,
                WeightedGreeks = new GreekStats() // Initialize with zero values
                {
                    Delta = 0,
                    Gamma = 0,
                    Theta = 0,
                    Vega = 0,
                    Rho = 0
                }
            };
        }

        var totalVolume = trades.Sum(t => t.Quantity);
        var totalValue = trades.Sum(t => t.Price * t.Quantity);
        var avgPrice = totalValue / totalVolume;
        var ivs = trades.Select(t => t.IV);
        var ivSpread = ivs.Max() - ivs.Min();

        var weightedGreeks = trades.Aggregate(new GreekStats(), (acc, trade) =>
        {
            var weight = (trade.Price * trade.Quantity) / totalValue;
            return new GreekStats
            {
                Delta = acc.Delta + (decimal)trade.Delta * weight,
                Gamma = acc.Gamma + (decimal)trade.Gamma * weight,
                Theta = acc.Theta + (decimal)trade.Theta * weight,
                Vega = acc.Vega + (decimal)trade.Vega * weight,
                Rho = acc.Rho + (decimal)trade.Rho * weight
            };
        });

        return new TradeStatistics
        {
            TotalVolume = totalVolume,
            TotalValue = totalValue,
            AveragePrice = avgPrice,
            IVSpread = ivSpread,
            WeightedGreeks = weightedGreeks
        };
    }

    private OptionTrade GenerateTrade()
    {
        var symbol = Symbols[_random.Next(Symbols.Length)];
        var isCall = _random.Next(2) == 0;
        var type = isCall ? "call" : "put";
        var strike = Math.Round(_random.Next(50, 500) + _random.NextDouble(), 2);
        var expiration = DateTime.Now.AddDays(_random.Next(1, 360));
        var basePrice = (decimal)(_random.Next(1, 100) + _random.NextDouble());
        var trade = new OptionTrade
        {
            Timestamp = DateTime.Now,
            Symbol = symbol,
            Option = $"{symbol} {expiration:MMMyy} {strike} {type.ToUpper()}",
            Price = Math.Round(basePrice, 2),
            Quantity = _random.Next(1, 1000),
            Type = type,
            Strike = (decimal)strike,
            Expiration = expiration,
            IV = (decimal)Math.Round(_random.NextDouble(), 4),
            Delta = (decimal)Math.Round(_random.NextDouble() * (isCall ? 1 : -1), 4),
            Gamma = (decimal)Math.Round((decimal)_random.NextDouble() * 0.1m, 4),
            Theta = (decimal)Math.Round((decimal)_random.NextDouble() * -1m, 4),
            Vega = (decimal)Math.Round((decimal)_random.NextDouble() * 0.1m, 4),
            Rho = (decimal)Math.Round((decimal)_random.NextDouble() * 0.05m, 4)
        };
        return trade;
    }
}
