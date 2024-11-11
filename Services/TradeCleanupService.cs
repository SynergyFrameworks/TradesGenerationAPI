namespace TradesAPI.Services;

public class TradeCleanupService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly ITradeGenerator _tradeGenerator;
    private readonly ILogger<TradeCleanupService> _logger;

    public TradeCleanupService(
        ITradeGenerator tradeGenerator,
        ILogger<TradeCleanupService> logger)
    {
        _tradeGenerator = tradeGenerator;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trade Cleanup Service is starting.");

        _timer = new Timer(DoCleanup, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(5)); 

        return Task.CompletedTask;
    }

    private void DoCleanup(object state)
    {
        _logger.LogInformation("Performing trade data cleanup");
      
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trade Cleanup Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

