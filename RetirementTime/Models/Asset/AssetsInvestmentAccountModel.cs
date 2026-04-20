namespace RetirementTime.Models.Asset;

public class AssetsInvestmentAccountItemModel
{
    public long Id { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public long AccountTypeId { get; set; }
    public decimal? AdjustedCostBasis { get; set; }
    public decimal? CurrentTotalValue { get; set; }
    public bool UseIndividualHoldings { get; set; }
    public List<AssetsHoldingItemModel> Holdings { get; set; } = [];
}

public class AssetsHoldingItemModel
{
    public long Id { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public bool IsPubliclyTraded { get; set; }
    public decimal? CurrentValue { get; set; }
    public string TickerSymbol { get; set; } = string.Empty;
    public decimal? AdjustedCostBasis { get; set; }
}
