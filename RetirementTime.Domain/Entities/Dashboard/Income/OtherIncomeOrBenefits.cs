using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Domain.Entities.Dashboard.Income;

public class OtherIncomeOrBenefits
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public long? RetirementTimelineId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? Amount { get; set; } = 0;
    public int? FrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public RetirementTimeline? RetirementTimeline { get; set; }
    public Frequency? Frequency { get; set; }
}