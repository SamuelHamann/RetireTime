using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Entities.Dashboard.Asset;

public class AssetsInvestmentAccount
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public string AccountName { get; set; } = null!;
    public long AccountTypeId { get; set; }
    public decimal? AdjustedCostBasis { get; set; }
    public decimal? CurrentTotalValue { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public List<AssetsHolding> Holdings { get; set; } = new();
    public AccountType AccountType { get; set; } = null!;
}

public class AssetsHolding
{
    public long Id { get; set; }
    public long InvestmentAccountId { get; set; }
    public string AssetName { get; set; } = null!;
    public bool IsPubliclyTraded { get; set; }
    public decimal? CurrentValue { get; set; }
    public string TickerSymbol { get; set; } = null!;
    public decimal? AdjustedCostBasis { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public AssetsInvestmentAccount InvestmentAccount { get; set; } = null!;
}
