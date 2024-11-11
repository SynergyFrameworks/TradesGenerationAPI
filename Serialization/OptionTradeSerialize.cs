namespace TradesAPI.Models;

public class OptionTradeSerial
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public DateTime Timestamp { get; set; }

    public int StockId { get; set; }

    public string? Symbol { get; set; }

    public string? Option { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string? Type { get; set; }

    public decimal Strike { get; set; }

    public DateTime Expiration { get; set; }

    public decimal IV { get; set; }

    public decimal Delta { get; set; }

    public decimal Gamma { get; set; }

    public decimal Theta { get; set; }

    public decimal Vega { get; set; }

    public decimal Rho { get; set; }

    public virtual Stock? Stock { get; set; }
}
