using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;

namespace RetirementTime.Domain.Entities.Dashboard.Spending;

public class SpendingAssetsExpense
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal? Expense { get; set; }
    public int FrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public long? AssetsHomeId { get; set; }
    public long? AssetsInvestmentPropertyId { get; set; }
    public long? AssetsInvestmentAccountId { get; set; }
    public long? AssetsPhysicalAssetId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public Frequency Frequency { get; set; } = null!;
    public AssetsHome? AssetsHome { get; set; }
    public AssetsInvestmentProperty? AssetsInvestmentProperty { get; set; }
    public AssetsInvestmentAccount? AssetsInvestmentAccount { get; set; }
    public AssetsPhysicalAsset? AssetsPhysicalAsset { get; set; }
}
