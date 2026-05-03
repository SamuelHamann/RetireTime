using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;

namespace RetirementTime.Domain.Entities.Dashboard.Spending;

public class SpendingInvestmentExpense
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }

    public decimal? Amount { get; set; }
    public int FrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public long? InvestmentAccountId { get; set; }
    public long? RetirementTimelineId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public Frequency Frequency { get; set; } = null!;
    public AssetsInvestmentAccount? InvestmentAccount { get; set; }
    public RetirementTimeline? RetirementTimeline { get; set; }
}

