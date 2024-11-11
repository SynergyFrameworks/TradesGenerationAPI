using MessagePack;
namespace TradesAPI.Models;

[MessagePackObject]
public class OptionTrade
{
    [Key(0)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; }

    [Key(2)]
    public int StockId { get; set; }

    [Key(3)]
    public string? Symbol { get; set; }

    [Key(4)]
    public string? Option { get; set; }

    [Key(5)]
    public decimal Price { get; set; }

    [Key(6)]
    public int Quantity { get; set; }

    [Key(7)]
    public string? Type { get; set; }

    [Key(8)]
    public decimal Strike { get; set; }

    [Key(9)]
    public DateTime Expiration { get; set; }

    [Key(10)]
    public decimal IV { get; set; }

    [Key(11)]
    public decimal Delta { get; set; }

    [Key(12)]
    public decimal Gamma { get; set; }

    [Key(13)]
    public decimal Theta { get; set; }

    [Key(14)]
    public decimal Vega { get; set; }

    [Key(15)]
    public decimal Rho { get; set; }

    [IgnoreMember] 
    public virtual Stock? Stock { get; set; }
}