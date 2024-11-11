namespace TradesAPI.Models;

public class TradeStatistics
{
    public decimal TotalVolume { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal TotalValue { get; set; }
    public decimal IVSpread { get; set; }
    public GreekStats? WeightedGreeks { get; set; }
}
