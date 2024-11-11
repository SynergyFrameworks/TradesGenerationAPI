namespace TradesAPI.Services;

public interface ITradeGenerator
{
    Task StartGeneratingTrades(string connectionId);
    void StopGeneratingTrades(string connectionId);
}
