using TradesAPI.Data;
using TradesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace TradesAPI.Services;


public class DatabaseSeeder
{
    private readonly TradeDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(TradeDbContext context, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            if (!_context.Stocks.Any())
            {
                var stocks = new List<Stock>
                {
                    new Stock
                    {
                        Symbol = "AAPL",
                        CompanyName = "Apple Inc.",
                        CurrentPrice = 175.25M,
                        Beta = 1.21M,
                        MarketCap = 2850000000000M,
                        Volume = 75000000,
                        DailyHigh = 176.50M,
                        DailyLow = 174.20M,
                        YearlyHigh = 182.50M,
                        YearlyLow = 124.75M,
                        PE_Ratio = 28.5M,
                        Dividend_Yield = 0.65M
                    },
                    new Stock
                    {
                        Symbol = "MSFT",
                        CompanyName = "Microsoft Corporation",
                        CurrentPrice = 338.15M,
                        Beta = 0.95M,
                        MarketCap = 2520000000000M,
                        Volume = 45000000,
                        DailyHigh = 340.20M,
                        DailyLow = 337.10M,
                        YearlyHigh = 342.15M,
                        YearlyLow = 241.85M,
                        PE_Ratio = 32.8M,
                        Dividend_Yield = 0.85M
                    },
                    new Stock
                    {
                        Symbol = "GOOGL",
                        CompanyName = "Alphabet Inc.",
                        CurrentPrice = 125.75M,
                        Beta = 1.15M,
                        MarketCap = 1580000000000M,
                        Volume = 35000000,
                        DailyHigh = 126.80M,
                        DailyLow = 124.90M,
                        YearlyHigh = 129.95M,
                        YearlyLow = 85.65M,
                        PE_Ratio = 25.4M,
                        Dividend_Yield = 0M
                    },
                    new Stock
                    {
                        Symbol = "AMZN",
                        CompanyName = "Amazon.com Inc.",
                        CurrentPrice = 128.85M,
                        Beta = 1.25M,
                        MarketCap = 1320000000000M,
                        Volume = 55000000,
                        DailyHigh = 129.90M,
                        DailyLow = 127.75M,
                        YearlyHigh = 135.25M,
                        YearlyLow = 88.35M,
                        PE_Ratio = 65.8M,
                        Dividend_Yield = 0M
                    },
                    new Stock
                    {
                        Symbol = "TSLA",
                        CompanyName = "Tesla Inc.",
                        CurrentPrice = 245.75M,
                        Beta = 2.15M,
                        MarketCap = 780000000000M,
                        Volume = 125000000,
                        DailyHigh = 248.50M,
                        DailyLow = 243.20M,
                        YearlyHigh = 285.75M,
                        YearlyLow = 152.45M,
                        PE_Ratio = 78.5M,
                        Dividend_Yield = 0M
                    }
                };

                await _context.Stocks.AddRangeAsync(stocks);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Database seeded with initial stock data");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }
}


