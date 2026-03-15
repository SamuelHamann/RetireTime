namespace RetirementTime.Domain.Entities.BeginnerGuide.Assets;

public class BeginnerGuideAssetsStockData
{
    public int Id { get; set; }
    public int InvestmentAccountId { get; set; }
    public required string TickerSymbol { get; set; }
    public decimal Amount { get; set; }
    
    // Navigation properties
    public required BeginnerGuideAssetsInvestmentAccount InvestmentAccount { get; set; }
}

