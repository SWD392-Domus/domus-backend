namespace Domus.Service.Models.Responses;

public class QuotationPriceHistory
{
    public DateTime UpdatedAt { get; set; }
    public double CurrentPrice { get; set; }
    public double PreviousPrice { get; set; }
    public double PriceChange { get; set; }
    public double PriceChangeInPercentage { get; set; }
}