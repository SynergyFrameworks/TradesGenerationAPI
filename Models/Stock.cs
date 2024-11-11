namespace TradesAPI.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string? Symbol { get; set; }
        public string? CompanyName { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Beta { get; set; }
        public decimal MarketCap { get; set; }
        public int Volume { get; set; }
        public decimal DailyHigh { get; set; }
        public decimal DailyLow { get; set; }
        public decimal YearlyHigh { get; set; }
        public decimal YearlyLow { get; set; }
        public decimal PE_Ratio { get; set; }
        public decimal Dividend_Yield { get; set; }

        public virtual ICollection<OptionTrade>? OptionTrades { get; set; }
    }

}
